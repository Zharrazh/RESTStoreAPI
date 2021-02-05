using AutoMapper;
using RESTStoreAPI.Data.DbModels;
using RESTStoreAPI.Models.User;
using RESTStoreAPI.Models.User.Update;
using RESTStoreAPI.Services;
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

            CreateMap<UserUpdateRequest, UserDbModel>()
                .ForMember(x => x.Roles, opt => opt.MapFrom(x => RoleUtils.GetRoleString(x.Roles)))
                .ForMember(x=> x.PasswordHash, opt=> opt.MapFrom<SaltHashResolver>());
        }
    }

    class SaltHashResolver : IValueResolver<UserUpdateRequest, UserDbModel, string>
    {
        private readonly IPasswordService passwordService;

        public SaltHashResolver(IPasswordService passwordService)
        {
            this.passwordService = passwordService;
        }

        public string Resolve(UserUpdateRequest source, UserDbModel destination, string destMember, ResolutionContext context)
        {
            return passwordService.SaltHash(source.Password);
        }
    }
}
