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
        public PasswordService(IHashService hashService, string salt)
        {
            this.hashService = hashService;
            this.salt = salt;
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
