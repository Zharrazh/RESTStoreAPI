using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.Common
{
    public class BadRequestType : Dictionary<string, string[]>
    {
        public BadRequestType() : base()
        {
        }
        public BadRequestType(ActionContext context)
        {
            ConstructErrorMessages(context.ModelState);
        }
        public BadRequestType(ModelStateDictionary modelState)
        {
            ConstructErrorMessages(modelState);
        }

        private void ConstructErrorMessages  (ModelStateDictionary modelState)
        {
            foreach (var keyModelStatePair in modelState)
            {
                var key = keyModelStatePair.Key;
                if (key != string.Empty && char.IsUpper(key[0]))
                {
                    key = char.ToLower(key[0]) + key.Substring(1);
                }
                var errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    var errorMessages = new string[errors.Count];
                    for (var i = 0; i<errors.Count; i++)
                    {
                        errorMessages[i] = GetErrorMessage(errors[i]);
                    }

                    Add(key, errorMessages);
                }
            }
        }

        static string GetErrorMessage(ModelError error)
        {
            return string.IsNullOrEmpty(error.ErrorMessage) ?
                "The input was not walid" : error.ErrorMessage;
        }
    }

    public class BadRequestTypeExample : IExamplesProvider<BadRequestType>
    {
        public BadRequestType GetExamples()
        {
            var example = new BadRequestType
            {
                { "name", new[] { "first name error", "second name error" } },
                { "desc", new[] { "once desc error" } },
                { "", new[] { "summary error 1", "summary error 2" } }
            };
            return example;
        }
    }

}
