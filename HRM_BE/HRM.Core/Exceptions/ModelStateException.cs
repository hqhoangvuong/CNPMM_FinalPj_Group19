using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HRM.Core.Exceptions
{
    public class ModelStateException : Exception
    {
        public ModelStateDictionary ModelState { get; }

        public ModelStateException(string key, string errorMessage)
        {
            ModelState = new ModelStateDictionary();
            ModelState.AddModelError(key, errorMessage);
        }

        public ModelStateException(ModelStateDictionary modelState)
        {
            ModelState = modelState;
        }
    }
}
