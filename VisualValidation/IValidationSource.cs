using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace VisualValidation
{
    public interface IValidationSource : INotifyPropertyChanged
    {
        IDictionary<string, Func<string>> ValidationFuncs{ get;}
    }
}