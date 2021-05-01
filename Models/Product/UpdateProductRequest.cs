using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.Product
{
    public class UpdateProductRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
    public class UpdateProductRequestExample : IExamplesProvider<UpdateProductRequest>
    {
        public UpdateProductRequest GetExamples()
        {
            var example = new UpdateProductRequest
            {
                Name = "updated product name",
                Description = "pdated lalala product desc",
                Price = 123.001m,
                CategoryId = 4
            };
            return example;
        }
    }
}
