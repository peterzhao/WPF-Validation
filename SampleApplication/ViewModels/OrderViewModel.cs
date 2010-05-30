using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SampleApplication.Domain;

namespace SampleApplication.ViewModels
{
    public class OrderViewModel  :PropertyChangedNotifier
    {
        private Order order;


        public OrderViewModel()
        {
            order = CreateOrder();
            SubmitCommand = new SimpleCommand
                                {
                                    ExecuteDelegate = p =>
                                                          {
                                                              if (!Order.ValidationEnabled)
                                                              {
                                                                  Order.ValidationEnabled = true;
                                                              }

                                                              if(Order.IsValid) Submit();
                                                          },
                                    CanExecuteDelegate = p =>
                                                             {
                                                                 return !Order.ValidationEnabled || Order.IsValid;
                                                             }

                                };

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

       public  SimpleCommand SubmitCommand { get; set; }


    }
}