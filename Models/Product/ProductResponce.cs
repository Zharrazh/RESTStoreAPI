using RESTStoreAPI.Models.Common;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.Product
{
    public class ProductResponce
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public  List<PictureInfoResponce> Pics { get; set; } = new List<PictureInfoResponce>();
    }

    public class ProductResponceExample : IExamplesProvider<ProductResponce>
    {
        public ProductResponce GetExamples()
        {
            var example = new ProductResponce
            {
                Id = 1,
                Name= "product name",
                Description= "lalala product desc",
                Price = 20.001m,
                CategoryId = 2,
                Pics = new List<PictureInfoResponce>
                {
                    new PictureInfoResponce
                    {
                        Id=2,
                        FileName="first_pic.jpg",
                        Path = "/sad/saddsa/first_pic.jpg"
                    },
                    new PictureInfoResponce
                    {
                        Id=3,
                        FileName="second_pic.jpg",
                        Path = "/sad/saddsa/second_pic.jpg"
                    }
                }
            };
            return example;
        }
    }
}
