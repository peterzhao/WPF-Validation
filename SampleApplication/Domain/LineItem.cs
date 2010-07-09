using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using VisualValidation;

namespace SampleApplication.Domain
{
    public class LineItem : IValidationSource
    {
       
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        
        public string this[string validationField]
        {
            get
            {
                if (validationField == "Quantity")
                    return Quantity < 0 ? "Quantity should not be less than 0." : string.Empty;
                return string.Empty;
            }
        }
        public bool IsValid
        {
            get { return this["Quantity"] == string.Empty; }
        }

        public Product Product
        {
            get { return product; }
            set
            {
                if (product != value)
                {
                    product = value;
                    NotifyPropertyChanged(MethodBase.GetCurrentMethod());
                }
            }
        }

        public int Quantity
        {
            get { return quantity; }
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    NotifyPropertyChanged(MethodBase.GetCurrentMethod());
                }
            }
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void NotifyPropertyChanged(MethodBase method)
        {
            NotifyPropertyChanged(method.Name.Substring(4));
        }
        private Product product;
        private int quantity;
    }
}