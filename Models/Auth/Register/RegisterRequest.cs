using Microsoft.AspNetCore.Mvc;
using RESTStoreAPI.Utils.ValidationAttributes.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.Auth.Register
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        [LoginIsFree]
        public string Login { get; set; }
        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        public bool IsAdmin { get; set; } = false;
    }
}
