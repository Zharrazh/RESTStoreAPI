using Microsoft.Extensions.Options;
using RESTStoreAPI.Data.DbModels;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Setup.Sieve
{
    public class ApplicationSieveProcessor : SieveProcessor
    {
        public ApplicationSieveProcessor(
            IOptions<SieveOptions> options,
            ISieveCustomSortMethods customSortMethods,
            ISieveCustomFilterMethods customFilterMethods)
            : base(options, customSortMethods, customFilterMethods)
        {
        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<UserDbModel>(p => p.Login)
                .CanFilter()
                .CanSort();
            mapper.Property<UserDbModel>(p => p.Profile.Name).HasName(nameof(UserDbModel.Profile.Name))
                .CanFilter()
                .CanSort();
            mapper.Property<UserDbModel>(p => p.IsActive)
                .CanFilter()
                .CanSort();
            mapper.Property<UserDbModel>(p => p.Profile.Created).HasName(nameof(UserDbModel.Profile.Created))
                .CanFilter()
                .CanSort();
            mapper.Property<UserDbModel>(p => p.Profile.Updated).HasName(nameof(UserDbModel.Profile.Updated))
                .CanFilter()
                .CanSort();
            mapper.Property<UserDbModel>(p => p.Profile.LastLoginDate).HasName(nameof(UserDbModel.Profile.LastLoginDate))
                .CanFilter()
                .CanSort();


            mapper.Property<ProductDbModel>(p => p.Name).CanFilter().CanFilter();
            mapper.Property<ProductDbModel>(p => p.Price).CanFilter().CanSort();
            mapper.Property<ProductDbModel>(p => p.CategoryId).CanFilter();


            return mapper;
        }
    }
}
