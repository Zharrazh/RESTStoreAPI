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
    public class CategoryTreeElementResponce
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsNode { get; set; }

        [SwaggerSchema("null if category is not node")]
        public List<CategoryTreeElementResponce> ChildCategories { get; set; }
    }

    public class CategoryTreeElementResponceExample : IExamplesProvider<CategoryTreeElementResponce>
    {
        public CategoryTreeElementResponce GetExamples()
        {
            return new CategoryTreeElementResponce
            {
                Id = 22321,
                IsNode = true,
                Name = "example name",
                ChildCategories = {}
            };
        }
    }
}
