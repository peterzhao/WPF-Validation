using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace VisualValidation
{
    public interface IValidationSource : INotifyPropertyChanged
    {
        string this[string validationFieldd]{ get;}
    }
}