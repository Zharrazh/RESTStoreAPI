using RESTStoreAPI.Data.DbModels;
using RESTStoreAPI.Services;
using RESTStoreAPI.Utils;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.User
{
    public class UserFullInfoResponce
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public List<string> Roles { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime Updated { get; set; }
        [Required]
        public DateTime LastLoginDate { get; set; }
    }


    public class UserFullInfoResponceExample : IExamplesProvider<UserFullInfoResponce>
    {
        public UserFullInfoResponce GetExamples()
        {
            var now = DateTime.UtcNow;
            var example = new UserFullInfoResponce
            {
                Id = 1337,
                Login = "Login",
                Name = "Вася Пупкин",
                Roles = new List<string>{Roles.AdminRoleName, Roles.UserRoleName},
                Created = now,
                Updated =now,
                LastLoginDate= now
            };
            return example;
        }
    }
}
