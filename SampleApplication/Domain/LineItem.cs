using System;
using System.Collections.Generic;
using System.Reflection;
using VisualValidation;

namespace SampleApplication.Domain
{
    public class LineItem : ValidationSource
    {
        private Product product;
        private int quantity;
        private readonly IDictionary<string, Func<string>> validationFuncs;

        public LineItem()
        {
            validationFuncs = new Dictionary<string, Func<string>>
                                  {
                                      {"Quantity", () => Quantity < 0? "Quantity should not be less than 0.": string.Empty},
                                  };
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

        public override IDictionary<string, Func<string>> ValidationFuncs
        {
            get { return validationFuncs; }
        }
    }
}