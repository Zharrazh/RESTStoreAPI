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
        public string Name { get; set; }
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
        [Required]
        public DateTime LastLoginDate { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime Updated { get; set; }
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
            builder.HasData(new UserDbModel
            {
                Id = 1,
                Login = "Admin",
                Name = "Admin",
                IsActive = true,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                PasswordHash = passwordService.SaltHash("1234"),
                Roles = "au"
            });
        }
    }
}
