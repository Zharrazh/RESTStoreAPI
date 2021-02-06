using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.User.Update
{
    public class UserUpdatePasswordRequest
    {
        [Required]
        [StringLength(50)]
        public string NewPassword { get; set; }
    }
}
