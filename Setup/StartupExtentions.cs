using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RESTStoreAPI.Models.Common;
using RESTStoreAPI.Setup.Config.Models;
using RESTStoreAPI.Setup.Sieve;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RESTStoreAPI.Setup
{
    public static class StartupExtentions
    {

        public static IServiceCollection AddAuthStartup(this IServiceCollection services, AuthConfigModel authConfig)
        {
            services
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.Key));
                    options.TokenValidationParameters.ValidIssuer = authConfig.Issuer;
                    options.TokenValidationParameters.ValidAudience = authConfig.Audience;
                    options.TokenValidationParameters.IssuerSigningKey = key;
                    options.TokenValidationParameters.ValidateIssuerSigningKey = true;
                    options.TokenValidationParameters.ValidateLifetime = true;
                    options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
                });
            
            return services.AddAuthorization();

        }

        public static IApplicationBuilder UseAuthStartup(this IApplicationBuilder app)
        {
            return app.UseAuthentication().UseAuthorization();
        }
        public static IServiceCollection AddSwaggerStartup(this IServiceCollection services)
        {
            return services.AddSwaggerGen(builder =>
            {
                builder.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.\n
                      Enter 'Bearer' [space] and then your token in the text input below.\n
                      Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                builder.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                });
            });
        }

        public static IApplicationBuilder UseSwaggerStartup(this IApplicationBuilder app)
        {
            return app.UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Store API V1");
            });
        }

        public static IServiceCollection AddSieveStartup(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .Configure<SieveOptions>(configuration.GetSection("Sieve"))
                .AddScoped<ISieveCustomSortMethods, SieveCustomSortMethods>()
                .AddScoped<ISieveCustomFilterMethods, SieveCustomFilterMethods>()
                .AddScoped<ISieveProcessor, ApplicationSieveProcessor>();
        }

        public static IServiceCollection AddFixValidationStartup(this IServiceCollection services)
        {
            return services.Configure<ApiBehaviorOptions>(a =>
            {
                a.InvalidModelStateResponseFactory = context =>
                {
                    var badReqObj = new BadRequestType(context);

                    return new BadRequestObjectResult(badReqObj)
                    {
                        ContentTypes = { "application/problem+json", "application/problem+xml" },
                    };
                };
            });
        }
    }
}
