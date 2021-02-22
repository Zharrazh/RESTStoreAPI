using System;
using Swashbuckle.AspNetCore.Annotations;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;

namespace RESTStoreAPI.Models.Category
{
    public class CategoryMinResponce
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsNode { get; set; }

    }

    public class CategoryMinResponceExample : IExamplesProvider<CategoryMinResponce>
    {
        public CategoryMinResponce GetExamples()
        {
            return new CategoryMinResponce
            {
                Id = 22321,
                IsNode = true,
                Name = "example name"
            };
        }
    }
}
