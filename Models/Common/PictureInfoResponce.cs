using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.Common
{
    public class PictureInfoResponce
    {
        [Required]
        public string Path { get; set; }
        [Required]
        public string FileName { get; set; }
    }

    public class PictureInfoResponceExample : IExamplesProvider<PictureInfoResponce>
    {
        public PictureInfoResponce GetExamples()
        {
            return new PictureInfoResponce
            {
                FileName = "blaBlaBla-asd123142sad.jpeg",
                Path = @"example\blaBlaBla-asd123142sad.jpeg"
            };
        }
    }
}
