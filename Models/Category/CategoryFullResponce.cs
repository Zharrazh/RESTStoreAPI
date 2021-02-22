using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.Category
{
    public class CategoryFullResponce
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public int? ParentId { get; set; }

        [Required]
        public bool IsNode { get; set; }

        [SwaggerSchema("null if category is not node")]
        public List<CategoryMinResponce> ChildCategories { get; set; }
    }

    public class CategoryFullResponceExample : IExamplesProvider<CategoryFullResponce>
    {
        public CategoryFullResponce GetExamples()
        {
            return new CategoryFullResponce
            {
                Id = 1337,
                Name = "my category name",
                Description = "lalala",
                IsNode = true,
                ParentId = 132,
                ChildCategories = new List<CategoryMinResponce>
                {
                    new CategoryMinResponce
                    {
                        Id = 213132,
                        IsNode =false,
                        Name = "child leaf"
                    }
                }
            };
        }
    }
};
