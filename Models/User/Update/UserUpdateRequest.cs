using RESTStoreAPI.Services;
using RESTStoreAPI.Utils.ValidationAttributes.User;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.User.Update
{
    public class UserUpdateRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [AvailableRoleStrings]
        public List<string> Roles { get; set; }
        [Required]
        public bool IsActive { get; set; }

    }
    public class UserUpdateRequestExample : IExamplesProvider<UserUpdateRequest>
    {
        public UserUpdateRequest GetExamples()
        {
            var example = new UserUpdateRequest
            {
                Name = "my new name",
                Roles = new List<string>
                {
                    Roles.AdminRoleName,
                    Roles.UserRoleName
                },
                IsActive = true
            };
            return example;
        }
    }
}
