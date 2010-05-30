﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VisualValidation;

namespace SampleApplication.Domain
{
    public abstract class ValidationSource : PropertyChangedNotifier, IValidationSource
    {
        protected bool validationEnabled;
        public abstract IDictionary<string, Func<string>> ValidationFuncs { get; }

       

        public  bool ValidationEnabled
        {
            get { return validationEnabled; }
            set
            {
                if(value != validationEnabled)
                {
                    validationEnabled = value;
                    ValidationEnabledChanged.Invoke(this, null);
                    NotifyPropertyChanged("ValidationEnabled");
                }
            }
        }

        public bool IsValid
        {
            get { return ValidationFuncs.Values.All(func => string.IsNullOrEmpty(func.Invoke())); }
        }

        public event EventHandler ValidationEnabledChanged = delegate{};

       
    }
}