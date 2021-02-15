using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.Auth
{
    public class TokenInfoResponce
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public DateTime Expires { get; set; }
    }

    public class TokenInfoResponceExample : IExamplesProvider<TokenInfoResponce>
    {
        public TokenInfoResponce GetExamples()
        {
            var example = new TokenInfoResponce
            {
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9."+
                        "eyJ1bmlxdWVfbmFtZSI6IkFkbWluIiwibmFtZWlkIjoiMSIsImdpdmVuX25hbWUiOiJoZWxsbyIsIn" +
                        "JvbGUiOiJhZG1pbiIsIm5iZiI6MTYxMzM3Njc0NCwiZXhwIjoxNjEzNDQxNTQ0LCJpYXQiOjE2MTMz" +
                        "NzY3NDQsImlzcyI6IlN0b3JlQVBJIEF1dGgiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAvIn0."+
                        "YhJgKjANQrcIvsF7-RrvbQTa7Qyn4b4pMVVLcIVjjpE",
                Expires = DateTime.UtcNow
            };
            return example;
        }
    }
}
