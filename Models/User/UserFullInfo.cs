using RESTStoreAPI.Data.DbModels;
using RESTStoreAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.User
{
    public class UserFullInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public List<string> Roles { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public static class UserDbModelExtention
    {
        public static UserFullInfo ToFullInfo(this UserDbModel userDbModel)
        {
            return new UserFullInfo
            {
                Id = userDbModel.Id,
                Name = userDbModel.Name,
                Login = userDbModel.Login,
                Roles = RoleUtils.GetRoleList(userDbModel.Roles),
                Created = userDbModel.Created,
                Updated = userDbModel.Updated
            };
        }
    }
}
