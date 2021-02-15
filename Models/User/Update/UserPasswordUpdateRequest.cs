using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.User.Update
{
    public class UserPasswordUpdateRequest
    {
        [Required]
        [StringLength(50)]
        public string NewPassword { get; set; }
    }

    public class UserUpdatePasswordRequestExample : IExamplesProvider<UserPasswordUpdateRequest>
    {
        public UserPasswordUpdateRequest GetExamples()
        {
            var example = new UserPasswordUpdateRequest
            {
                NewPassword = "my_new_password"
            };
            return example;
        }
    }
}
