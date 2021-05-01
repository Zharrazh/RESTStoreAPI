using Sieve.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.Product
{
    public class ProductSieveModel : SieveModel
    {
        [SwaggerParameter("can filter: name, price, categoryId")]
        public virtual new string Filters
        {
            get { return base.Filters; }
            set { base.Filters = value; }
        }

        [SwaggerParameter("can sorts: name, price")]
        public virtual new string Sorts
        {
            get { return base.Sorts; }
            set { base.Sorts = value; }
        }
    }
}
