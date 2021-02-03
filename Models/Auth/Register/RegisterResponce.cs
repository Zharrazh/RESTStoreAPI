using RESTStoreAPI.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.Auth.Register
{
    public class RegisterResponce
    {
        [Required]
        public TokenInfoResponce TokenInfo { get; set; }
        [Required]
        public UserFullInfoResponce UserInfo { get; set; }

    }
}
