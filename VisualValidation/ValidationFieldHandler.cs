using System;
using System.ComponentModel;
using System.Diagnostics;

namespace VisualValidation
{
    public class ValidationFieldHandler
    {
        private readonly IUIElement uiElement;
        private  IValidationSource validationSource;
        private readonly string validationFiled;
        private readonly IValidationContainer validationContainer;

        public ValidationFieldHandler(IUIElement uiElement, string validationFiled, IValidationContainer validationContainer)
        {
            this.uiElement = uiElement;
            this.validationFiled = validationFiled;
            this.validationContainer = validationContainer;
            validationContainer.ValidationSourceChanged += OnValidationSourceChanged;
            validationContainer.ValidationEnabledChanged += OnValidationEnabledChanged;
            SetValidationSourceHandler();
            Debug.WriteLine(string.Format("Validation Handler constructed. validaton source: {0},  field {1} ", validationSource, uiElement));
        }

        private void OnValidationEnabledChanged(object sender, EventArgs e)
        {
            SetValidationResult(); 
        }

        private void  OnValidationSourceChanged(object sender, EventArgs e)
       {
           SetValidationSourceHandler();
           Debug.WriteLine(string.Format("ValiationSource changed:  current once: {0}, for field {1} ", validationSource, uiElement));

       }

        private void SetValidationSourceHandler()
        {
            var newSource = validationContainer.ValidationSource ;
            Debug.WriteLine(string.Format("ValiationSource set from filed handler: new one: {0}, current once: {1}, for field {2} ", newSource, validationSource, uiElement));
            if (newSource != validationSource)
            {
                UnRegisterEventFromValidationSource();
                validationSource = newSource;
                RegisterEventFromValiationSource();
            }
            SetValidationResult();

        }

        private void SetValidationResult()
        {
            var errorMessage = "";
                if (validationContainer.ValidationEnabled 
                    && validationSource != null 
                    && validationSource.ValidationFuncs!=null && validationSource.ValidationFuncs.Keys.Contains(validationFiled))
                    errorMessage = validationSource.ValidationFuncs[validationFiled].Invoke();

            uiElement.SetValue(ValidationContainer.ValidationResultProperty, new ValidationResult(errorMessage));
        }

       
        public void RegisterEventFromValiationSource()
        {
            if (validationSource != null) validationSource.PropertyChanged += OnValidationSourcePropertyChanged;
        }

        public void UnRegisterEventFromValidationSource()
        {
            if (validationSource != null) validationSource.PropertyChanged -= OnValidationSourcePropertyChanged;
        }

        private void OnValidationSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == validationFiled )
            {
                SetValidationResult();
            }
        }
    }
}
