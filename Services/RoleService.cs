using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Services
{
    public interface IRoleService
    {
        public string GetRoleName(char roleKey);
        public List<string> GetRoleNames(string roleKeysString);
        public char GetRoleKey(string roleName);
        public string GetRoleKeys(List<string> roleNames);
        public bool IsCorrectRoleName(string roleName);
        public bool IsCorrectRoleKey(char roleKey);
        public List<string> GetAllRoleNames();
        public List<char> GetAllRoleKeys();
    }
    public class RoleService : IRoleService
    {
        public List<char> GetAllRoleKeys()
        {
            return Roles.KeyNameParis.Keys.ToList();
        }

        public List<string> GetAllRoleNames()
        {
            return Roles.KeyNameParis.Values.ToList();
        }

        public char GetRoleKey(string roleName)
        {
            if (!IsCorrectRoleName(roleName))
            {
                throw new ArgumentException("Invalid role name", nameof(roleName));
            }

            return Roles.KeyNameParis.FirstOrDefault(x => x.Value == roleName).Key;
        }

        public string GetRoleKeys(List<string> roleNames)
        {
            if (roleNames.Any(x=> !IsCorrectRoleName(x)))
            {
                throw new ArgumentException("List of field names contains invalid values", nameof(roleNames));
            }
            var res = new string(roleNames.Select(roleStr => Roles.KeyNameParis.FirstOrDefault(x => x.Value == roleStr).Key).ToArray());
            return res;
        }

        public string GetRoleName(char roleKey)
        {
            if (!IsCorrectRoleKey(roleKey))
            {
                throw new ArgumentException("Invalid role key", nameof(roleKey));
            }

            return Roles.KeyNameParis[roleKey];
        }

        public List<string> GetRoleNames(string roleKeysString)
        {
            if (roleKeysString.Any(x => !IsCorrectRoleKey(x))){
                throw new ArgumentException("Role key string contains invalid keys", nameof(roleKeysString));
            }
            return roleKeysString
                .Select(key => Roles.KeyNameParis[key])
                .ToList();
        }

        public bool IsCorrectRoleKey(char roleKey)
        {
            return Roles.KeyNameParis.ContainsKey(roleKey);
        }

        public bool IsCorrectRoleName(string roleName)
        {
            return Roles.KeyNameParis.ContainsValue(roleName);
        }
    }

    public class Roles
    {
        public const string AdminRoleName = "admin";
        public const string UserRoleName = "user";
        public static Dictionary<char, string> KeyNameParis { get; } = new Dictionary<char, string>
        {
            ['a'] = AdminRoleName,
            ['u'] = UserRoleName
        };
    }
}
