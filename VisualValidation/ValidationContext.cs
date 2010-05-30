using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace VisualValidation
{
    public class ValidationContext: IValidationContext
    {
        readonly Dictionary<string,string> validationResults = new Dictionary<string, string>();


        public void UpdateValidationResult(string fieldName, string errorMessage)
        {
            if (validationResults.Keys.Contains(fieldName))
                validationResults[fieldName] = errorMessage;
            else
                validationResults.Add(fieldName, errorMessage);
            
            NotifyPropertyChanged("IsValid");
            NotifyPropertyChanged("ErrorMessages");
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate{};

        public bool IsValid
        {
            get
            {
                return validationResults.Keys.ToList().All(key => string.IsNullOrEmpty(validationResults[key]));
            }
        }
        public List<string> ErrorMessages
        {
            get
            {
                return validationResults.Values.ToList();
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
