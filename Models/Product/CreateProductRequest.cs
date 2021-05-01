using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.Product
{
    public class CreateProductRequest
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

    public class CreateProductRequestExample : IExamplesProvider<CreateProductRequest>
    {
        public CreateProductRequest GetExamples()
        {
            var example = new CreateProductRequest
            {
                Name = "new product name",
                Description = "lalala product desc",
                Price = 21.001m,
                CategoryId = 3
            };
            return example;
        }
    }
}
