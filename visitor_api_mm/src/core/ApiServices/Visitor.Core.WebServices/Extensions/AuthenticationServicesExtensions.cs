using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Visitor.Core.WebServices.Extensions;

public static class AuthenticationServicesExtensions
{
    public static void AddIdentityAuthorization(this IServiceCollection services)
    {
        //var a = AppSettings.JwtSettings.ValidIssuer;
        IdentityModelEventSource.ShowPII = true;
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

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = AppSettings.JwtSettings.ValidIssuer,
                    ValidAudience = AppSettings.JwtSettings.ValidAudience
                };
            });
    }
}
