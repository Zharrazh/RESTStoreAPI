using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.Category.Update
{
    public class UpdateCategoryRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public int? ParentId { get; set; }
    }

    public class UpdateCategoryRequestExample : IExamplesProvider<UpdateCategoryRequest>
    {
        public UpdateCategoryRequest GetExamples()
        {
            return new UpdateCategoryRequest
            {
                Name = "new category name",
                Description = "if you want update root category that set parentId == null",
                ParentId = 1
            };
        }
    }
}
