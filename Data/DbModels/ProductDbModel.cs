using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Data.DbModels
{
    [Index(nameof(Name), IsUnique = true)]
    public class ProductDbModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public virtual CategoryLeafDbModel Category { get; set; }

        public virtual List<PictureInfoDbModel> Pics { get; set; } = new List<PictureInfoDbModel>();

    }
}
