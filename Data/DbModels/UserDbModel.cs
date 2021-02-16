using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RESTStoreAPI.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Data.DbModels
{
    public class UserDbModel
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Login { get; set; }
        [Required]
        [MaxLength(50)]
        public string PasswordHash { get; set; }
        [Required]
        [MaxLength(10)]
        public string Roles { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public virtual UserProfileDbModel Profile { get; set; }

    }

    public class UserProfileDbModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public DateTime LastLoginDate { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime Updated { get; set; }
        public int UserId { get; set; }
        public virtual UserDbModel User { get; set; }

    }

    public class UserConfiguration : IEntityTypeConfiguration<UserDbModel>
    {
        private readonly IPasswordService passwordService;
        public UserConfiguration(IPasswordService passwordService)
        {
            this.passwordService = passwordService;
        }
        void IEntityTypeConfiguration<UserDbModel>.Configure(EntityTypeBuilder<UserDbModel> builder)
        {
            builder.HasIndex(x => x.Login).IsUnique();
            var now = DateTime.UtcNow;
            builder.HasData(new UserDbModel()
            {
                Id = 1,
                Login = "Admin",
                IsActive = true,
                PasswordHash = passwordService.SaltHash("1234"),
                Roles = "au"
            }); 
        }
    }

    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfileDbModel>
    {
        public void Configure(EntityTypeBuilder<UserProfileDbModel> builder)
        {
            var now = DateTime.UtcNow;
            builder.HasData(new UserProfileDbModel()
            {   
                Id = 1,
                UserId = 1,
                Name = "Admin",
                Created = now,
                LastLoginDate = now,
                Updated = now
                
            });
        }
    }
}
