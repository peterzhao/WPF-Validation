using System.Reflection;
using SampleApplication.Domain;
using VisualValidation;

namespace SampleApplication.ViewModels
{
    public class AddressViewModel : PropertyChangedNotifier
    {
        private readonly Address address;
        private bool validationEnabled;

        public AddressViewModel(Address address)
        {
            this.address = address;

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
                }
            }
        }

      
        public Address Address
        {
            get{ return address;}
        }
       
    }
}