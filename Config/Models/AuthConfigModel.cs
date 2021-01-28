using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Config.Models
{
    public class AuthConfigModel
    {
        public string Key { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string PasswordSalt { get; set; }
        public int Expires { get; set; }
    }
}
