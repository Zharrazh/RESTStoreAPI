using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.Auth.GetToken
{
    public class GetTokenRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
