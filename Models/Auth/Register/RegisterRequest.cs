using Microsoft.AspNetCore.Mvc;
using RESTStoreAPI.Utils.ValidationAttributes.User;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DefaultValue(false)]
        public bool IsAdmin { get; set; } = false;
    }

    public class RegisterRequestExample : IExamplesProvider<RegisterRequest>
    {
        public RegisterRequest GetExamples()
        {
            return new RegisterRequest
            {
                Login = "NewUselLogin",
                Name = "NewUserName",
                Password = "12345",
                IsAdmin = false
            };
        }
    }
}
