using AutoMapper;
using RESTStoreAPI.Models.Auth;
using RESTStoreAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Setup.AutoMapper
{
    public class TokenProfile : Profile
    {
        public TokenProfile()
        {
            CreateMap<TokenInfo, TokenInfoResponce>();
        }
    }
}
