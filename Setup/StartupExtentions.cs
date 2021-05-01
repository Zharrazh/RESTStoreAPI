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
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Reflection;
using System.Text;


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

            services.AddSwaggerGen(builder =>
            {
                builder.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.RelativePath}");

                builder.EnableAnnotations();

                builder.ExampleFilters();

                //builder.OperationFilter<AddHeaderOperationFilter>();
                //builder.OperationFilter<AddResponseHeadersFilter>();

                builder.OperationFilter<AppendAuthorizeToSummaryOperationFilter>(); // Adds "(Auth)" to the summary so that you can see which endpoints have Authorization
                                                                                    // or use the generic method, e.g. c.OperationFilter<AppendAuthorizeToSummaryOperationFilter<MyCustomAttribute>>();

                // add Security information to each operation for OAuth2

                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = @"Put <strong>ONLY</strong> your JWT Bearer token on textbox below!</br>
                                    It add in headers ""Auhorization: Bearer your_token""",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                builder.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                builder.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        { jwtSecurityScheme, Array.Empty<string>() }
                    });

                //builder.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    Description = @"JWT Authorization header using the Bearer scheme.
                //      Enter 'Bearer' [space] and then your token in the text input below.
                //      Example: 'Bearer 12345abcdef'",
                //    Name = "Authorization",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = "Bearer"
                //});

                //builder.AddSecurityRequirement(new OpenApiSecurityRequirement()
                //{
                //    {
                //      new OpenApiSecurityScheme
                //      {
                //        Reference = new OpenApiReference
                //          {
                //            Type = ReferenceType.SecurityScheme,
                //            Id = "Bearer"
                //          },
                //          Scheme = "oauth2",
                //          Name = "Bearer",
                //          In = ParameterLocation.Header,

                //        },
                //        Array.Empty<string>()
                //      }
                //});

                


            });

            return services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
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
