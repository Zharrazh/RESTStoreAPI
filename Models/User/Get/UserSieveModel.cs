using Sieve.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.User.Get
{
    public class UserSieveModel : SieveModel
    {
        [SwaggerParameter("can filter: login, name, isActive, created, updated, lastLoginDate")]
        public virtual new string Filters
        {
            get { return base.Filters; }
            set { base.Filters = value; }
        }

        [SwaggerParameter("can sorts: login, name, isActive, created, updated, lastLoginDate")]
        public virtual new string Sorts {
            get { return base.Sorts; }
            set { base.Sorts = value; }
        }
    }

}
