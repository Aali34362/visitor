using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Visitor.Core.InfraServices.Extensions;

public static class JwtServiceCollectionExtensions
{
    public static IServiceCollection AddIamJwt(this IServiceCollection services, IConfiguration config)
    {
        // Accept either ValidIssuer/ValidAudience or Issuer/Audience (your code currently uses Valid*)
        var issuer = config["Jwt:ValidIssuer"] ?? config["Jwt:Issuer"] ?? "Inforter.Inventory";
        var audience = config["Jwt:ValidAudience"] ?? config["Jwt:Audience"] ?? "account";

        // Use Jwt:Secret if present; else fall back to Jwt:ClientSecret so you don’t have to add a new key right now
        var secret = config["Jwt:ClientSecret"];
        if (string.IsNullOrWhiteSpace(secret))
            throw new InvalidOperationException("JWT secret is missing. Set Jwt:Secret (preferred) or Jwt:ClientSecret.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tvp = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = key,

            ClockSkew = TimeSpan.Zero
        };

        // Register singletons so the same instances are used by your app service and JwtBearer
        services.AddSingleton(creds);
        services.AddSingleton(tvp);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o => o.TokenValidationParameters = tvp);

        services.AddAuthorization();

        return services;
    }
}