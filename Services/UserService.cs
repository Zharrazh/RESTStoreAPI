using Microsoft.EntityFrameworkCore;
using RESTStoreAPI.Data;
using RESTStoreAPI.Data.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Services
{
    public interface IUserService
    {
        Task<UserDbModel> GetUserAsync(string login, string password);
    }
    public class UserService : IUserService
    {
        private readonly DatabaseContext db;
        private readonly IPasswordService passwordService;

        public UserService(DatabaseContext db, IPasswordService passwordService)
        {
            this.db = db;
            this.passwordService = passwordService;
        }

        public async Task<UserDbModel> GetUserAsync(string login, string password)
        {
            return await db.Users.FirstOrDefaultAsync(x => x.Login == login && passwordService.VerifyPassword(x.PasswordHash, password));
        }
    }
}
