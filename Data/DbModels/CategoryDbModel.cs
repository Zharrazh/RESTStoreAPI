using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Data.DbModels
{
    [Index(nameof(Name),IsUnique = true)]
    public class CategoryDbModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public virtual CategoryNodeDbModel Parent { get; set; }

        public virtual PictureInfoDbModel Pic { get; set; }
    }

    public class CategoryNodeDbModel : CategoryDbModel
    {
        public virtual List<CategoryDbModel> ChildCategories { get; set; } = new List<CategoryDbModel>();
    }

    public class CategoryLeafDbModel : CategoryNodeDbModel
    {
    }

    public class CategoryNodeConfiguration : IEntityTypeConfiguration<CategoryNodeDbModel>
    {
        public void Configure(EntityTypeBuilder<CategoryNodeDbModel> builder)
        {
            builder.HasMany(x => x.ChildCategories).WithOne(x => x.Parent).OnDelete(DeleteBehavior.Cascade);
        }
 
    }
}
