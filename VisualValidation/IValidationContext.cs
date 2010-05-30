using System.Collections.Generic;
using System.ComponentModel;

namespace VisualValidation
{
    public interface IValidationContext : INotifyPropertyChanged
    {
        void UpdateValidationResult(string fieldName, string errorMessage);
        bool IsValid { get; }
        List<string> ErrorMessages { get; }
    }
}