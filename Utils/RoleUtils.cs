using RESTStoreAPI.Utils.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Utils
{
    public static class RoleUtils
    {
        public static List<string> GetRoleList(string roles)
        {
            return roles
                .Where(key => RoleConstants.Roles.ContainsKey(key))
                .Select(key => RoleConstants.Roles[key])
                .ToList();
        }

        public static string GetRoleString(List<string> roles)
        {
            return (string)roles.Select(roleStr => RoleConstants.Roles.FirstOrDefault(x => x.Value == roleStr).Key);
        }
    }
}
