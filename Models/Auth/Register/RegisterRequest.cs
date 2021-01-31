using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.Auth.Register
{
    public class RegisterRequest
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
