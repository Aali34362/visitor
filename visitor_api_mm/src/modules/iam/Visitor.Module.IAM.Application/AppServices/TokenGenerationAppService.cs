using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Visitor.Module.IAM.Application.AppServices;

public interface ITokenGenerationAppService
{
    Task<Result<TokenResponse>> GenerateTokenAsync(GenerateTokenCommand command);
}

public class TokenGenerationAppService : ITokenGenerationAppService
{
    private readonly IValidationService _validationService;
    private readonly IIdentityUserBusinessService _userBusinessService;
    private readonly IIdentityUserRoleBusinessService _userRoleBusinessService;
    private readonly SigningCredentials _signingCredentials;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly short _accessExpiryMinutes;
    private readonly short _refreshExpiryMinutes;

    private readonly string _bootstrapToken;

    public TokenGenerationAppService(
        IValidationService validationService,
        IIdentityUserBusinessService userBusinessService,
        IIdentityUserRoleBusinessService userRoleBusinessService,
        SigningCredentials signingCredentials,
        TokenValidationParameters tokenValidationParameters,
        IConfiguration config)
    {
        _validationService = validationService;
        _userBusinessService = userBusinessService;
        _userRoleBusinessService = userRoleBusinessService;
        _signingCredentials = signingCredentials;
        _tokenValidationParameters = tokenValidationParameters;
        _issuer = config["Jwt:ValidIssuer"];
        _audience = config["Jwt:ValidAudience"];
        _bootstrapToken = config["Jwt:ClientSecret"];
        _accessExpiryMinutes = config.GetValue<short>("Jwt:accessExpiryMinutes");
        _refreshExpiryMinutes = config.GetValue<short>("Jwt:refreshExpiryMinutes");
    }

    public async Task<Result<TokenResponse>> GenerateTokenAsync(GenerateTokenCommand command)
    {
        var validationResult = await _validationService.ValidateAsync(command);
        if (!validationResult.IsSuccess)
            return Result<TokenResponse>.Failure(validationResult.Error);

        switch (command.GrantType.ToLowerInvariant())
        {
            case "client_credentials":
                return await HandleClientCredentialsFlow(command);

            case "password":
                return await HandlePasswordFlow(command);

            case "refresh_token":
                return await HandleRefreshTokenFlow(command);

            default:
                return Result<TokenResponse>.Failure(
                    ErrorDetail.Business("Invalid grant_type", "GrantType"));
        }
    }

    private static bool SecureEquals(string a, string b)
    {
        var aBytes = System.Text.Encoding.UTF8.GetBytes(a);
        var bBytes = System.Text.Encoding.UTF8.GetBytes(b);
        return aBytes.Length == bBytes.Length && CryptographicOperations.FixedTimeEquals(aBytes, bBytes);
    }

    private Task<Result<TokenResponse>> HandleClientCredentialsFlow(GenerateTokenCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.ClientSecret) ||
            !SecureEquals(command.ClientSecret, _bootstrapToken))
        {
            return Task.FromResult(Result<TokenResponse>.Failure(
                ErrorDetail.Business("Invalid client credential", "BootstrapToken")));
        }

        var serviceName = string.IsNullOrWhiteSpace(command.ClientId) ? "svc.bootstrap" : command.ClientId;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, serviceName),
            new(ClaimTypes.Name, "Service.Admin"),
            new(ClaimTypes.Role, "Service.Admin"),
            new(ClaimTypes.System, serviceName),
            //new("scope", "user.create")
        };

        var token = GenerateToken(claims);
        return Task.FromResult(Result<TokenResponse>.Success(token));
    }

    private async Task<Result<TokenResponse>> HandlePasswordFlow(GenerateTokenCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Username) ||
            string.IsNullOrWhiteSpace(command.Password))
        {
            return Result<TokenResponse>.Failure(
                ErrorDetail.Business("Username/password required", "Username"));
        }

        var user = await _userBusinessService
            .ValidateUserPasswordAsync(command.Username, command.Password);

        if (user is null)
        {
            return Result<TokenResponse>.Failure(
                ErrorDetail.Business("Invalid username/password", "Username"));
        }

        var roles = await _userRoleBusinessService.GetAllAsync(new() { User_Id = user.Id }, 1, 100);

        var serviceName = string.IsNullOrWhiteSpace(command.ClientId)
           ? "svc.bootstrap"
           : command.ClientId;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email.ToString()),
            new(ClaimTypes.System, serviceName),
            new(ClaimTypes.Name, user.UserName)
        };
        claims.AddRange(roles.Items.Select(r => new Claim(ClaimTypes.Role, r.Role_Nm)));

        var token = GenerateToken(claims); // always issues refresh, see method
        return Result<TokenResponse>.Success(token);
    }

    private async Task<Result<TokenResponse>> HandleRefreshTokenFlow(GenerateTokenCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.RefreshToken))
        {
            return Result<TokenResponse>.Failure(
                ErrorDetail.Business("Refresh token required", "RefreshToken"));
        }

        var principal = ValidateRefreshToken(command.RefreshToken);
        if (principal == null)
        {
            return Result<TokenResponse>.Failure(
                ErrorDetail.Business("Invalid refresh token", "RefreshToken"));
        }

        // FindFirstValue is an extension in System.Security.Claims
        var userIdStr = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdStr, out var userId))
        {
            return Result<TokenResponse>.Failure(
                ErrorDetail.Business("Invalid subject in token", "sub"));
        }

        var user = await _userBusinessService.GetByIdAsync(userId);
        if (user is null)
        {
            return Result<TokenResponse>.Failure(
                ErrorDetail.Business("User not found", "sub"));
        }

        var roles = await _userRoleBusinessService.GetAllAsync(new() { User_Id = user.Id }, 1, 100);

        // preserve correlation_id from refresh token if present
        Guid? existingCorrelation = null;
        var cid = principal.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.SessionId)?.Value;
        if (Guid.TryParse(cid, out var parsed)) existingCorrelation = parsed;

        var serviceName = string.IsNullOrWhiteSpace(command.ClientId)
           ? "svc.bootstrap"
           : command.ClientId;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email.ToString()),
            new(ClaimTypes.System, serviceName),
            new(ClaimTypes.Name, user.UserName)
        };
        claims.AddRange(roles.Items.Select(r => new Claim(ClaimTypes.Role, r.Role_Nm)));

        var token = GenerateToken(claims, existingCorrelation);
        return Result<TokenResponse>.Success(token);
    }

    private TokenResponse GenerateToken(IEnumerable<Claim> baseClaims, Guid? sessionId = null)
    {
        var now = DateTime.UtcNow;
        var session_Id = sessionId ?? Guid.NewGuid();

        var accessClaims = new List<Claim>(baseClaims)
        {
            new(CustomClaimTypes.SessionId, session_Id.ToString("D"))
        };

        var refreshClaims = new List<Claim>(baseClaims)
        {
            new(CustomClaimTypes.SessionId, session_Id.ToString("D")),
            new(CustomClaimTypes.IsRefreshToken, "true")
        };

        var accessJwt = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: accessClaims,
            notBefore: now,
            expires: now.AddMinutes(_accessExpiryMinutes),
            signingCredentials: _signingCredentials
        );

        var refreshJwt = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: refreshClaims,
            notBefore: now,
            expires: now.AddMinutes(_refreshExpiryMinutes),
            signingCredentials: _signingCredentials
        );

        var handler = new JwtSecurityTokenHandler();

        return new TokenResponse
        {
            Access_Token = handler.WriteToken(accessJwt),
            Expires_In = _accessExpiryMinutes,
            Refresh_Token = handler.WriteToken(refreshJwt),
            Refresh_Expires_In = _refreshExpiryMinutes,
            Token_Type = "Bearer"
        };
    }

    private ClaimsPrincipal ValidateRefreshToken(string refreshToken)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var principal = handler.ValidateToken(refreshToken, _tokenValidationParameters, out var validatedToken);

            if (validatedToken is JwtSecurityToken jwt &&
                jwt.Claims.Any(c => c.Type == CustomClaimTypes.IsRefreshToken && c.Value == "true"))
            {
                return principal;
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
}


/*
    Audience: What is the target of this token. 
    In other words which services, apis, products should accept this token as access token for the service. 
    They may be many valid tokens in the world, but not all of those tokens have been granted by the user 
    (or resource owner) to allow access to the resources saved in the product services. 
    A token valid for Google drive should not be accepted for GMail, even if both of them have the same issuer, 
    they’ll have different audiences. Why? Because an user may have given access
    to a 3rd party service to access their GMail, but not their documents in Drive.

    Issuer: Who created the token. 
    This can be verified by using the well-known openid configuration endpoint and public keys. 
    Since issuers are tied to DNS entries/url paths, each issuer must be unique. 
    Two services can’t both be the same issuer. 
    Tokens issued by Google will have a different issuer than the ones issued by Authress.

    --------------------------------------------------------------------
    
    {
       // ...Some other claim
       iss: "MyKnowledgeCenter.com",
       aud: "Abu.questioning.com",
       // ...Some more other claims
    }
    First thing is abount issuer, which represent as iss. 
    This indicate that where the whole Jwt you have come from. 
    For ex: We have a request for access token at https://abu.Identity.com, 
    but the issuer could be MyKnowledgeCenter.com, or https://abu.Identity.com or anything else.
    We have freedom to just indicate those as we code the identity centralize server ourself.

    In short, iss is just a Jwt claim that have nothing difference from other, 
    except it's meaning is to indicate that this jwt was self-declared that it 
    came from an issuer that called MyKnowledgeCenter.com from this example.

    About audience, again, it's just a claim in Jwt, that was intended to set as we wish, 
    represented for one or a collection of which services that the Jwt itself intended to use for.

    I love example: I have 2 microservices is Catalog and UserProfile, 
    that require client to have a Jwt was issued at MyKnowledgeCenter.com to access their resources. 
    If on those 2, validating on audience is required (as we can set it), then, 
    even if the sign is valid, but aud was lack of Catalog, client cannot access Catalog microservice resources. 
    The same applied for all others.

    Okay, so where's the client ?
    Well, that's indicate something as setting but not re-presented in the Jwt.
    Imagine, we have a centralize authentication server, but we only intend to serve our own services, and clients.
    Not the whole Internet world, right ?
    So, specifying those specific clients would benefit, as I just want to serve an app on iOs, 
    another from android word and a website of our own. 
    Therefore, 3 clients. If any request that came from any other clients that have informations doesn't match one of those 3,
    we rejected them immediately.
    */