using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace VisualValidation
{
    public class ValidationResult 
    {
        private string errorMessage;
        
        public bool IsValid { get; set;}

        public static ValidationResult EmptyValidationResult = new ValidationResult();

        public ValidationResult(string errorMessage)
        {
            this.errorMessage = errorMessage;
            IsValid = string.IsNullOrEmpty(errorMessage);
        }

        public ValidationResult():this(string.Empty){}

        public string ErrorMessage
        {
            get { return errorMessage; }
        }

        

        public bool Equals(ValidationResult other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.errorMessage, errorMessage);
        }

       

        public override string ToString()
        {
            return string.Format("error message: {0}", ErrorMessage);
        }
    }
}
