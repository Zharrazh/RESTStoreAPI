using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RESTStoreAPI.Data;
using RESTStoreAPI.Models.Common;
using RESTStoreAPI.Services;
using RESTStoreAPI.Setup;
using RESTStoreAPI.Setup.Config.Models;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace RESTStoreAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var authConfig = Configuration.GetSection("Auth").Get<AuthConfigModel>();
            var connectionString = Configuration.GetSection("Connections").GetValue<string>("Default");

            services.AddSingleton<IHashService, HashService>();
            services.AddSingleton<IPasswordService>(x => new PasswordService(x.GetRequiredService<IHashService>(), authConfig.PasswordSalt));
            services.AddScoped<IAuthService, AuthService>();

            services.AddSieveStartup(Configuration.GetSection("Sieve"));

            services.AddAuthStartup(authConfig);

            services.AddDbContext<DatabaseContext>(options => options.UseSqlite(connectionString));

            services.AddSwaggerStartup();

            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();

            services.AddFixValidationStartup(); // After AddCotrollers!

            services.AddHttpContextAccessor();

            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerStartup();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthStartup();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
