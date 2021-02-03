using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.Auth
{
    public class TokenInfoResponce
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public DateTime Expires { get; set; }
    }
}
