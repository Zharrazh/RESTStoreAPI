using RESTStoreAPI.Models.Category.Update;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.Category.Post
{
    public class CreateCategoryRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public int? ParentId { get; set; }
    }

    public class CreateCategoryRequestExample : IExamplesProvider<CreateCategoryRequest>
    {
        public CreateCategoryRequest GetExamples()
        {
            return new CreateCategoryRequest
            {
                Name = "new catgory",
                Description = "if you want create root category that set parentId == null",
                ParentId = 1
            };
        }
    }
}
