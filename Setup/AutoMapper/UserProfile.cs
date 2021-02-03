using AutoMapper;
using RESTStoreAPI.Data.DbModels;
using RESTStoreAPI.Models.User;
using RESTStoreAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Setup.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDbModel, UserFullInfoResponce>().ForMember(x=>x.Roles, opt => opt.MapFrom(x => RoleUtils.GetRoleList(x.Roles)));
        }
    }
}
