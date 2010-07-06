using System.ComponentModel;
using System.Reflection;

namespace VisualValidation
{
    public class PropertyChangedNotifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void NotifyPropertyChanged(MethodBase method)
        {
            NotifyPropertyChanged(method.Name.Substring(4));
        }
    }
}