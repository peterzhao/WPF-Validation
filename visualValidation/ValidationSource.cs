﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisualValidation
{
    public abstract class ValidationSource : PropertyChangedNotifier, IValidationSource
    {
        public abstract IDictionary<string, Func<string>> ValidationFuncs { get; }

        public bool IsValid
        {
            get { return ValidationFuncs.Values.All(func => string.IsNullOrEmpty(func.Invoke())); }
        }

       
       
    }
}