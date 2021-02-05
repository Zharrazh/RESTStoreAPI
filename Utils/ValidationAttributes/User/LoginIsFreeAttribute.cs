using RESTStoreAPI.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Utils.ValidationAttributes.User
{
    public class LoginIsFreeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DatabaseContext db = (DatabaseContext)validationContext.GetService(typeof(DatabaseContext));
            string login = (string)value;
            if (db.Users.Any(x => x.Login == login))
            {
                return new ValidationResult("A user with this login already exists");
            }
            return ValidationResult.Success;
        }
    }
}
