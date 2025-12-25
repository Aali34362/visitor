using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Visitor.Core.WebServices.Extensions;

public static class AuthenticationServicesExtensions
{
    public static void AddIdentityAuthorization(this IServiceCollection services)
    {
        /*
         RSA privateRsa = RSA.Create();
         privateRsa.ImportFromPem(File.ReadAllText("jwt_private.key"));
         
         RSA publicRsa = RSA.Create();
         publicRsa.ImportFromPem(File.ReadAllText("jwt_public.key"));
         
         var signingKey = new RsaSecurityKey(privateRsa) { KeyId = "auth-key-1" };
         var validationKey = new RsaSecurityKey(publicRsa) { KeyId = "auth-key-1" };
         
         builder.Services.AddSingleton(new SigningCredentials(
             signingKey,
             SecurityAlgorithms.RsaSha256
         ));
         
         builder.Services.AddSingleton(new TokenValidationParameters
         {
             ValidateIssuerSigningKey = true,
             IssuerSigningKey = validationKey,
             ValidateIssuer = true,
             ValidateAudience = true,
             ValidateLifetime = true,
             ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
             ValidAudience = builder.Configuration["Jwt:ValidAudience"],
             ClockSkew = TimeSpan.Zero
         });
         */

        IdentityModelEventSource.ShowPII = true;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.IdentitySettings.ClientSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);        
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = AppSettings.JwtSettings.ValidIssuer,
            ValidAudience = AppSettings.JwtSettings.ValidAudience,
            ClockSkew = TimeSpan.Zero
        };
        services.AddSingleton(creds);
        services.AddSingleton(tokenValidationParameters);
        services
            .AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = AppSettings.JwtSettings.Authority;
                options.SaveToken = false;
                // Require HTTPS for metadata endpoints
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = tokenValidationParameters;
            });
    }
}
