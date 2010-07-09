using System;
using System.Collections.Generic;
using System.Reflection;
using VisualValidation;

namespace SampleApplication.Domain
{
    public class Address : ValidationSource
    {
        private readonly IDictionary<string, Func<string>> validationFuncs;
        private string address1;
        private string address2;
        private string city;
        private string postalCode;
        private string province;

        public Address()
        {
            validationFuncs = new Dictionary<string, Func<string>>
                                  {
                                      {"Address1", () => string.IsNullOrEmpty(Address1) ? "Address1 should not be empty" : string.Empty},
                                      {"City", () => string.IsNullOrEmpty(City) ? "City should not be empty" : string.Empty},
                                      {"Province", () => string.IsNullOrEmpty(Province) ? "Province should not be empty" : string.Empty},
                                      {"PostalCode", () => string.IsNullOrEmpty(PostalCode) ? "Postal code should not be empty" : string.Empty}
                                  };
        }

        public string Address1
        {
            get { return address1; }
            set
            {
                if (address1 != value)
                {
                    address1 = value;
                    NotifyPropertyChanged(MethodBase.GetCurrentMethod());
                }
            }
        }

        public string Address2
        {
            get { return address2; }
            set
            {
                if (address2 != value)
                {
                    address2 = value;
                    NotifyPropertyChanged(MethodBase.GetCurrentMethod());
                }
            }
        }

        public string City
        {
            get { return city; }
            set
            {
                if (city != value)
                {
                    city = value;
                    NotifyPropertyChanged(MethodBase.GetCurrentMethod());
                }
            }
        }

        public string Province
        {
            get { return province; }
            set
            {
                if (province != value)
                {
                    province = value;
                    NotifyPropertyChanged(MethodBase.GetCurrentMethod());
                }
            }
        }

        public string PostalCode
        {
            get { return postalCode; }
            set
            {
                if (postalCode != value)
                {
                    postalCode = value;
                    NotifyPropertyChanged(MethodBase.GetCurrentMethod());
                }
            }
        }

        protected override IDictionary<string, Func<string>> ValidationFuncs
        {
            get { return validationFuncs; }
        }
    }
}