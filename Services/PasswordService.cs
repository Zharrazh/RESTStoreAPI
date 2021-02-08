using Microsoft.Extensions.Options;
using RESTStoreAPI.Setup.Config.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Services
{
    public interface IPasswordService
    {
        public string SaltHash(string password);
        public bool VerifyPassword(string hash, string password);
    }
    public class PasswordService : IPasswordService
    {
        private readonly string salt;
        private readonly IHashService hashService;
        public PasswordService(IHashService hashService, IOptionsSnapshot<AuthConfigModel> authConfigModelAcc)
        {
            this.hashService = hashService;
            this.salt = authConfigModelAcc.Value.PasswordSalt;
        }
        public string SaltHash(string password)
        {
            return hashService.Hash(salt + password + salt);
        }
        
        public bool VerifyPassword(string hash, string password)
        {
            return hash == SaltHash(password);
        }
    }
}
