using System;
using System.Collections.Generic;
using System.ComponentModel;
using VisualValidation;

namespace Tests
{
    public class StubValidationSource : IValidationSource
    {

        public StubValidationSource()
        {
            ValidationFuncs = new Dictionary<string, Func<string>>();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate{};

        
        public IDictionary<string, Func<string>> ValidationFuncs   { get; set; }
        
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string this[string validationFieldd]
        {
            get
            {
                if (ValidationFuncs == null || !ValidationFuncs.Keys.Contains(validationFieldd)) return string.Empty;
                return ValidationFuncs[validationFieldd].Invoke();
            }
        }
    }
}