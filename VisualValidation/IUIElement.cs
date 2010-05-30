using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace VisualValidation
{
    public interface IUIElement
    {
        object GetValue(DependencyProperty dependencyProperty);
        void SetValue(DependencyProperty dependencyProperty, object value);
        void AddHandler(RoutedEvent routedEvent, Delegate handler);
        void RemoveHandler(RoutedEvent routedEvent, Delegate handler);
        DependencyObject DependencyObject { get; }
    }
}
