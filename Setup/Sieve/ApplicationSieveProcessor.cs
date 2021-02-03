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
            mapper.Property<UserDbModel>(p => p.IsActive)
                .CanFilter();
            mapper.Property<UserDbModel>(p => p.Login)
                .CanFilter()
                .CanSort();
            mapper.Property<UserDbModel>(p => p.Name)
                .CanFilter()
                .CanSort();
            mapper.Property<UserDbModel>(p => p.Created)
                .CanFilter()
                .CanSort();
            mapper.Property<UserDbModel>(p => p.Updated)
                .CanFilter()
                .CanSort();

            return mapper;
        }
    }
}
