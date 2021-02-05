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
            modelBuilder.ApplyConfiguration(new UserConfiguration(passwordService));
        }
    }
}
