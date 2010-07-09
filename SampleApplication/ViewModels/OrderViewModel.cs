using System.Collections.Generic;
using System.Reflection;
using SampleApplication.Domain;
using VisualValidation;

namespace SampleApplication.ViewModels
{
    public class OrderViewModel : PropertyChangedNotifier
    {
        private Order order;
        private bool validationEnabled;
        private AddressViewModel addressViewModel;

        public OrderViewModel()
        {
            order = CreateOrder();
            addressViewModel = new AddressViewModel(order.DeliveryAddress);

            order.PropertyChanged += (s, e) =>
                                         {
                                             if (e.PropertyName == "NeedDelivery")
                                             {
                                                 UpdateAddressVadliationEnabled();
                                             }
                                         };
            SubmitCommand = new SimpleCommand
                                {
                                    ExecuteDelegate = p =>
                                                          {
                                                              ValidationEnabled = true;
                                                              if (Order.IsValid) Submit();
                                                          },
                                    CanExecuteDelegate = p => !ValidationEnabled || Order.IsValid
                                };
            ResetCommand = new SimpleCommand
                               {
                                   ExecuteDelegate = p => ValidationEnabled = false
                               };

        }

        private void UpdateAddressVadliationEnabled()
        {
            addressViewModel.ValidationEnabled = ValidationEnabled && order.NeedDelivery.HasValue && order.NeedDelivery.Value;
        }

        public bool ValidationEnabled
        {
            get { return validationEnabled; }
            set
            {
                if (value != validationEnabled)
                {
                    validationEnabled = value;
                    NotifyPropertyChanged(MethodBase.GetCurrentMethod());
                    UpdateAddressVadliationEnabled();
                }
            }
        }

        private Order CreateOrder()
        {
            return new Order
                       {
                           DeliveryAddress = new Address(),
                           LineItems = new List<LineItem>
                                           {
                                               new LineItem{Product = new Product{Name = "LapTop101", Description = "LapTop 100", Price = 999.99m}, Quantity = 1},
                                               new LineItem{Product = new Product{Name = "Book", Description = "Book abc", Price = 29.4m}, Quantity = 1},
                                               new LineItem{Product = new Product{Name = "Disk", Description = "Disk 100G", Price = 57.95m}, Quantity = 1}
                                           }
                       };
        }

        private void Submit()
        {

        }

        public Order Order
        {
            get
            {
                return order;
            }

        }

        public AddressViewModel AddressViewModel { get { return addressViewModel; }
        }

        public SimpleCommand SubmitCommand { get; set; }
        public SimpleCommand ResetCommand { get; set; }


    }
}