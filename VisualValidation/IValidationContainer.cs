using System;

namespace VisualValidation
{
    public interface IValidationContainer
    {
        IValidationSource ValidationSource { get; set; }
        event EventHandler ValidationSourceChanged;
        event EventHandler ValidationEnabledChanged;
        bool ValidationEnabled { get; set; }
    }
}