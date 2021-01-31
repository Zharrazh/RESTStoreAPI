using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.Auth.GetToken
{
    public class GetTokenResponce
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public List<string> Roles { get; set; }
        public bool IsAdmin { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
