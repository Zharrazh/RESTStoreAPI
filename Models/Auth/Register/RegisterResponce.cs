using RESTStoreAPI.Models.User;
using Swashbuckle.AspNetCore.Filters;
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

    public class RegisterResponceExample : IExamplesProvider<RegisterResponce>
    {
        public RegisterResponce GetExamples()
        {
            return new RegisterResponce
            {
                TokenInfo = new TokenInfoResponceExample().GetExamples(),
                UserInfo = new UserFullInfoResponceExample().GetExamples()
            };
        }
    }
}
