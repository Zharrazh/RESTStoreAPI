using RESTStoreAPI.Data.DbModels;
using RESTStoreAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.User
{
    public class UserFullInfoResponce
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public List<string> Roles { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

}
