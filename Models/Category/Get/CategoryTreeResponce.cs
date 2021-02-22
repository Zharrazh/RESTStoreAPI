using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.Category.Get
{
    public class CategoryTreeResponce
    {
        public List<CategoryTreeElementResponce> RootCategories { get; set; }
    }

    public class CategoryTreeResponceExample : IExamplesProvider<CategoryTreeResponce>
    {
        public CategoryTreeResponce GetExamples()
        {
            return new CategoryTreeResponce
            {
                RootCategories = new List<CategoryTreeElementResponce>
               {
                   new CategoryTreeElementResponce
                   {
                       Id = 1,
                       Name = "root node name",
                       IsNode =true,
                       ChildCategories = new List<CategoryTreeElementResponce>
                       {
                           new CategoryTreeElementResponce()
                           {
                               Id = 2,
                               Name = "leaf child 1",
                               IsNode =false
                           }
                       }
                   }
               }
            };
        }
    }
}
