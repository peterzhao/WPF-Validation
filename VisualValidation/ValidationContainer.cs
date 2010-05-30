using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace VisualValidation
{
    /// <summary>
    /// ValidationContainer is a light weight validation framework which only takes care displaying visaul effect of validation results for each validation field.
    /// Don't use it to control Navigation. View Model should control it. 
    /// </summary>
    public class ValidationContainer : ContentControl, IValidationContainer
    {
        /// <summary>
        /// An object of IValidationSource which provide vilidation and normally is View Model.
        /// </summary>
        public static readonly DependencyProperty ValidationSourceProperty =
          DependencyProperty.Register("ValidationSource", typeof(IValidationSource),
                                      typeof(ValidationContainer),
                                      new FrameworkPropertyMetadata(null, ValidationSourcePropertySet));

        public IValidationSource ValidationSource
        {
            get { return GetValue(ValidationSourceProperty) as IValidationSource; }
            set { SetValue(ValidationSourceProperty, value); }
        }

        public void RaiseValidationSourceChangedEvent()
        {
            ValidationSourceChanged.Invoke(this, null);
        }

        public event EventHandler ValidationSourceChanged = delegate { };


        /// <summary>
        /// The name of a property of the IValidationSource. When this IValidationSource notifies this property changed, 
        /// ValidationContainer will validate this field with IValiationSource and set ValidationResult to the binded UI control.
        /// </summary>
        public static readonly DependencyProperty ValidationFieldProperty =
            DependencyProperty.RegisterAttached("ValidationField", typeof(string), typeof(ValidationContainer),
                                                new PropertyMetadata(string.Empty, ValidateFieldPropertySetOnChildElement));

        public static string GetValidationField(DependencyObject d)
        {
            return (string)d.GetValue(ValidationFieldProperty);
        }

        public static void SetValidationField(DependencyObject d, string value)
        {
            d.SetValue(ValidationFieldProperty, value);
        }


        /// <summary>
        /// Can be used to display validation result for the binded UI control.
        /// </summary>
        public static readonly DependencyProperty ValidationResultProperty =
            DependencyProperty.RegisterAttached("ValidationResult",
                                        typeof(ValidationResult),
                                        typeof(ValidationContainer),
                                        new FrameworkPropertyMetadata(null));

        public static ValidationResult GetValidationResult(DependencyObject d)
        {
            return (ValidationResult)d.GetValue(ValidationResultProperty);
        }

        public static void SetValidationResult(DependencyObject d, ValidationResult value)
        {
            d.SetValue(ValidationResultProperty, value);
        }

        /// <summary>
        /// used internally by valiation framework. 
        /// </summary>
        public static readonly DependencyProperty ValidationFieldHandlerProperty =
         DependencyProperty.RegisterAttached("ValidationFieldHandler",
                                     typeof(ValidationFieldHandler),
                                     typeof(ValidationContainer),
                                     new FrameworkPropertyMetadata(null));


      
        private static void ValidationSourcePropertySet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(ValidationSourceProperty, e.NewValue);
            var uiElement = d as IValidationContainer;
            if (uiElement != null)
            {
                Debug.WriteLine(string.Format("Validation source {0} set on {1}", e.NewValue, uiElement));
                uiElement.RaiseValidationSourceChangedEvent();
            }
        }




        private static void ValidateFieldPropertySetOnChildElement(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uiElement = d as FrameworkElement;
            if (uiElement == null) throw new ApplicationException("ValidationField property should be set on a FrameworkElement.");
            var filed = e.NewValue as string;

            var oldValidationFieldHandler = uiElement.GetValue(ValidationFieldHandlerProperty) as ValidationFieldHandler;
            if (oldValidationFieldHandler != null)
            {
                oldValidationFieldHandler.UnRegisterEventFromValidationSource();
                uiElement.SetValue(ValidationFieldHandlerProperty, null);
            }
            if (uiElement.IsLoaded)
            {
                SetValidationHandler(uiElement, filed);
            } else
            {
                uiElement.Loaded += (s, arg) => SetValidationHandler(uiElement, filed);
            }
        }

        private static void SetValidationHandler(FrameworkElement uiElement, string filed)
        {
            var oldValidationFieldHandler = uiElement.GetValue(ValidationFieldHandlerProperty) as ValidationFieldHandler;
            if (oldValidationFieldHandler == null)
            {
                var validationContainer = new UiTreeHelper().FindValidationContainer(uiElement);
                var validationFieldHandler = new ValidationFieldHandler(new UIElementWrapper(uiElement), filed, validationContainer);
                uiElement.SetValue(ValidationFieldHandlerProperty, validationFieldHandler);
            }
        }
    }
}