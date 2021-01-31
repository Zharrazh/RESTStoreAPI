using RESTStoreAPI.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Services
{
    public interface IRoleService
    {
        List<string> GetRoleList(string roles);
        string GetRoleString(List<string> roles);
    }
    public class RoleService : IRoleService
    {
        public List<string> GetRoleList(string roles)
        {
            return roles
                .Where(key => RoleConstants.Roles.ContainsKey(key))
                .Select(key => RoleConstants.Roles[key])
                .ToList();
        }

        public string GetRoleString(List<string> roles)
        {
            return (string)roles.Select(roleStr => RoleConstants.Roles.FirstOrDefault(x => x.Value == roleStr).Key);
        }
    }
}
