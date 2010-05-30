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
        public bool ValidationEnabled { get; set; }

        
        public IDictionary<string, Func<string>> ValidationFuncs   { get; set; }
        
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NotifyValidationEnabledChanged()
        {
            NotifyPropertyChanged("ValidationEnabled");
        }
    }
}