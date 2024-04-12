using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Rapido.Framework.Auth.Authenticator;
using Rapido.Framework.Auth.Policies;
using Rapido.Framework.Auth.Policies.Handlers;
using Rapido.Framework.Common;

namespace Rapido.Framework.Auth;

public static class Extensions
{
    private const string SectionName = "auth";

   public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
   {
      var section = configuration.GetSection(SectionName);
      var options = section.BindOptions<AuthOptions>();
      
      services.Configure<AuthOptions>(section);

      services.AddSingleton<IAuthenticator, Authenticator.Authenticator>();
      
      services
         .AddAuthentication(opt =>
         {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         }).AddJwtBearer(opt =>
         {
            opt.Authority = options.Authority;
            opt.Audience = options.Audience;
            opt.MetadataAddress = options.MetadataAddress;
            opt.SaveToken = options.SaveToken;
            opt.RefreshOnIssuerKeyNotFound = options.RefreshOnIssuerKeyNotFound;
            opt.RequireHttpsMetadata = options.RequireHttpsMetadata;
            opt.IncludeErrorDetails = options.IncludeErrorDetails;
            opt.TokenValidationParameters = new TokenValidationParameters
            {
               RequireAudience = options.RequireAudience,
               ValidIssuer = options.ValidIssuer,
               ValidateAudience = options.ValidateAudience,
               ValidateIssuer = options.ValidateIssuer,
               ValidateLifetime = options.ValidateLifetime,
               ValidateTokenReplay = options.ValidateTokenReplay,
               ValidateIssuerSigningKey = options.ValidateIssuerSigningKey,
               SaveSigninToken = options.SaveSigninToken,
               RequireExpirationTime = options.RequireExpirationTime,
               RequireSignedTokens = options.RequireSignedTokens,
               ClockSkew = TimeSpan.Zero,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey))
            };
         });
      
      services.AddScoped<IAuthorizationHandler, RoleRequirementHandler>();

      services.AddAuthorization(authOptions =>
      {
         authOptions.AddPolicy(Policies.Policies.Admin, 
            builder => builder.AddRequirements(new RoleRequirement("admin")));
         
         authOptions.AddPolicy(Policies.Policies.User, 
            builder => builder.AddRequirements(new RoleRequirement("user")));
      });

      return services;
   }
}