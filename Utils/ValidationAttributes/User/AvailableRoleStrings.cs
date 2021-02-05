﻿using RESTStoreAPI.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Utils.ValidationAttributes.User
{
    public class AvailableRoleStrings : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is List<string> roles)
            {
                bool isCorrect = roles.All(x => RoleUtils.IsCorrectRoleName(x));
                if (!isCorrect)
                {
                    return new ValidationResult("The list contains an incorrect role name");
                }
                return ValidationResult.Success;
            }
            else if (value is string role)
            {
                if (RoleUtils.IsCorrectRoleName(role))
                {
                    return new ValidationResult("Role name is not correct");
                }
            }
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
