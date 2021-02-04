using AutoMapper;
using RESTStoreAPI.Models.Auth;
using RESTStoreAPI.Models.Common;
using RESTStoreAPI.Services;
using RESTStoreAPI.Setup.Sieve;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Setup.AutoMapper
{
    public class PageProfile : Profile
    {
        public PageProfile()
        {
            CreateMap( typeof(PaginationResult<>) , typeof(PageResponce<>));

            CreateMap<PageInfo, PageInfoResponce>();
        }
    }
}
