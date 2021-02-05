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
            var res = new string(roles.Where(x => IsCorrectRoleName(x)).Select(roleStr => RoleConstants.Roles.FirstOrDefault(x => x.Value == roleStr).Key).ToArray());
            return res;
        }

        public static bool IsCorrectRoleName(string roleName)
        {
            return RoleConstants.Roles.ContainsValue(roleName);
        }

        public static bool IsCorrectRoleKey(char roleKey)
        {
            return RoleConstants.Roles.ContainsKey(roleKey);
        }
    }
}
