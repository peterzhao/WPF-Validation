using System;
using System.Windows;
using System.Windows.Controls;

namespace VisualValidation
{
    public class UIElementWrapper: IUIElement
    {
        private readonly UIElement uiElement;

        public UIElementWrapper(UIElement uiElement)
        {
            this.uiElement = uiElement;
        }

        public object GetValue(DependencyProperty dependencyProperty)
        {
            return uiElement.GetValue(dependencyProperty);
        }

        public void SetValue(DependencyProperty dependencyProperty, object value)
        {
            uiElement.SetValue(dependencyProperty, value);
        }

        public void AddHandler(RoutedEvent routedEvent, Delegate handler)
        {
            uiElement.AddHandler(routedEvent, handler);
        }

        public void RemoveHandler(RoutedEvent routedEvent, Delegate handler)
        {
            uiElement.RemoveHandler(routedEvent, handler);
        }

        public DependencyObject DependencyObject
        {
            get { return uiElement; }
        }

        public override string ToString()
        {
            var name = uiElement.GetValue(FrameworkElement.NameProperty);
            return uiElement + " " + name;
        }
    }
}