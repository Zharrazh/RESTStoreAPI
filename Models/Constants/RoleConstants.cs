using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.Constants
{
    public class RoleConstants
    {
        public const string AdminRoleName = "admin";
        public const string UserRoleName = "user";
        public static Dictionary<char, string> Roles { get; } = new Dictionary<char, string>
        {
            ['a'] = AdminRoleName,
            ['u'] = UserRoleName
        };
    }
}
