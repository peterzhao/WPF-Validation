using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace VisualValidation
{
    public interface IValidationSource : INotifyPropertyChanged
    {
        bool ValidationEnabled { get; }
        IDictionary<string, Func<string>> ValidationFuncs{ get;}
    }
}