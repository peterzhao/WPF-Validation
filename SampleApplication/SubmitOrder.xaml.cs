using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SampleApplication.ViewModels;

namespace SampleApplication
{
    /// <summary>
    /// Interaction logic for SubmitOrder.xaml
    /// </summary>
    public partial class SubmitOrder : UserControl
    {
        public SubmitOrder()
        {
            DataContext = new OrderViewModel();
            InitializeComponent();
        }
    }
}
