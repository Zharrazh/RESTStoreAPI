using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.Auth.GetToken
{
    public class GetTokenRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class GetTokenRequestExample : IExamplesProvider<GetTokenRequest>
    {
        public GetTokenRequest GetExamples()
        {
            return new GetTokenRequest
            {
                Login ="Admin",
                Password = "1234"
            };
        }
    }
}
