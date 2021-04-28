using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RESTStoreAPI.Services
{
    public interface IHashService
    {
        string Hash(string data);
        string Hash(IFormFile file);
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

        public string Hash(IFormFile file)
        {
            MemoryStream stream = new MemoryStream();
            file.OpenReadStream().CopyTo(stream);
            // compute md5 hash of the file's byte array.
            byte[] bytes = MD5.Create().ComputeHash(stream.ToArray());
            return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLower();
        }
    }
}
