using RESTStoreAPI.Utils.ValidationAttributes.User;
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
        [MaxLength(50)]
        public string Password { get; set; }
        [Required]
        public bool IsActive { get; set; }

    }
}
