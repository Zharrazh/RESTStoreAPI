using Microsoft.EntityFrameworkCore;
using RESTStoreAPI.Data.DbModels;
using RESTStoreAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Data
{
    public class DatabaseContext : DbContext
    {
        private readonly IPasswordService passwordService;
        public DbSet<UserDbModel> Users { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IPasswordService passwordService) : base(options)
        {
            this.passwordService = passwordService;
            //Database.EnsureDeleted();
            Database.EnsureCreated();

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.Entity<UserDbModel>().HasData(new UserDbModel {
                Id = 1,
                Login = "Admin",
                Name="Admin",
                IsActive= true,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                PasswordHash = passwordService.SaltHash("1234"),
                Roles = "au" 
            });

        }
    }
}
