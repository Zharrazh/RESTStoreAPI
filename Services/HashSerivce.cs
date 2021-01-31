using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RESTStoreAPI.Services
{
    public interface IHashService
    {
        string Hash(string data);
    }
    public class HashService : IHashService
    {
        public string Hash(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            using var algorithm = SHA512.Create();
            var hashBytes = algorithm.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
