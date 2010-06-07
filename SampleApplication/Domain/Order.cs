using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SampleApplication.Domain
{
    public class Order : ValidationSource
    {
        private Address deliveryAddress;
        private List<LineItem> lineItems;
        private bool? needDelivery;
        private string userName;
        private IDictionary<string, Func<string>> validationFuncs;

        public Order()
        {
            deliveryAddress = new Address();
            lineItems = new List<LineItem>();
            validationFuncs = new Dictionary<string, Func<string>>
                                  {
                                      {"UserName", () => string.IsNullOrEmpty(UserName)? "User Name should not be empty": string.Empty},
                                      {"NeedDelivery", () => NeedDelivery == null? "Please select if you need delivery.": string.Empty},
                                      {"DeliveryAddress", () => (NeedDelivery != null && NeedDelivery.Value && !DeliveryAddress.IsValid)? "Address is invalid.": string.Empty},
                                      {"LineItems", () => LineItems.Any(item => !item.IsValid) ? "Line items are invalid.": string.Empty} 
                                  };
        }

        public string UserName
        {
            get { return userName; }
            set
            {
                if (userName != value)
                {
                    userName = value;
                    NotifyPropertyChanged(MethodBase.GetCurrentMethod());
                }
            }
        }


        public bool? NeedDelivery
        {
            get { return needDelivery; }
            set
            {
                if (needDelivery != value)
                {
                    needDelivery = value;
                    SetValidationEnabledForAddress();
                    NotifyPropertyChanged(MethodBase.GetCurrentMethod());
                }
            }
        }

       

        public Address DeliveryAddress
        {
            get { return deliveryAddress; }
            set
            {
                if (deliveryAddress != value)
                {
                    deliveryAddress = value;
                    NotifyPropertyChanged(MethodBase.GetCurrentMethod());
                }
            }
        }


        public List<LineItem> LineItems
        {
            get { return lineItems; }
            set
            {
                if (lineItems != value)
                {
                    lineItems = value;
                    NotifyPropertyChanged(MethodBase.GetCurrentMethod());
                }
            }
        }


        public override IDictionary<string, Func<string>> ValidationFuncs
        {
            get { return validationFuncs; }
        }

        protected override void OnValidationEnabledChanged()
        {
            base.OnValidationEnabledChanged();
            if(LineItems != null)
            {
                LineItems.ForEach(item => item.ValidationEnabled = ValidationEnabled);
            }
            SetValidationEnabledForAddress();
        }

        private void SetValidationEnabledForAddress()
        {
            DeliveryAddress.ValidationEnabled = needDelivery != null && needDelivery.Value && ValidationEnabled;
        }
    }
}