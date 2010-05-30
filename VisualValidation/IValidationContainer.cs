using System;

namespace VisualValidation
{
    public interface IValidationContainer
    {
        IValidationSource ValidationSource { get; set; }
        void RaiseValidationSourceChangedEvent();
        event EventHandler ValidationSourceChanged;
    }
}