using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RESTStoreAPI.Data;
using RESTStoreAPI.Data.DbModels;
using RESTStoreAPI.Setup.Config.Models;
using RESTStoreAPI.Utils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RESTStoreAPI.Services
{
    public interface IAuthService
    {
        public bool IsAuthUser();
        public Task<UserDbModel> GetAuthUserAsync();
        public bool IsAuthUser(UserDbModel userDbModel);
        public bool AuthUserInRole(string roleName);
    }
    public class AuthService : IAuthService
    {
        private readonly HttpContext ctx;
        private readonly DatabaseContext db;

        public AuthService(DatabaseContext db, IHttpContextAccessor ctxAcc)
        {
            ctx = ctxAcc.HttpContext!;
            this.db = db;
        }

        public async Task<UserDbModel> GetAuthUserAsync()
        {
            if (ctx.User?.Identity?.Name == null)
                return null;
            else
            {
                return await db.Users.Include(x=> x.Profile).FirstOrDefaultAsync(u => u.Login == ctx.User.Identity.Name);
            }

        }

        public bool IsAuthUser(UserDbModel userDbModel)
        {

            if (ctx.User?.Identity?.Name == null)
                return false;
            else
            {
                return userDbModel.Login == ctx.User?.Identity?.Name;
            }
        }

        public bool IsAuthUser()
        {
            return ctx.User is not null;
        }

        public bool AuthUserInRole(string roleName)
        {
            return ctx.User.IsInRole(roleName);
        }

        
    }

    
}
