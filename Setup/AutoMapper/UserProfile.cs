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
            CreateMap<UserDbModel, UserFullInfoResponce>().ForMember(x=>x.Roles, opt => opt.MapFrom<RoleStringToRoleListResolver>());

            CreateMap<UserUpdateRequest, UserDbModel>()
                .ForMember(x => x.Roles, opt => opt.MapFrom<RoleListToRoleStringResolver>())
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

    class RoleStringToRoleListResolver : IValueResolver<UserDbModel, UserFullInfoResponce, List<string>>
    {
        private readonly IRoleService roleService;

        public RoleStringToRoleListResolver(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        public List<string> Resolve(UserDbModel source, UserFullInfoResponce destination, List<string> destMember, ResolutionContext context)
        {
            return roleService.GetRoleNames(source.Roles);
        }
    }

    class RoleListToRoleStringResolver : IValueResolver<UserUpdateRequest, UserDbModel, string>
    {
        private readonly IRoleService roleService;

        public RoleListToRoleStringResolver(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        public string Resolve(UserUpdateRequest source, UserDbModel destination, string destMember, ResolutionContext context)
        {
            return roleService.GetRoleKeys(source.Roles);
        }
    }
}
