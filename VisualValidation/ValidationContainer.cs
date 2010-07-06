using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace VisualValidation
{
    /// <summary>
    /// ValidationContainer is a light weight validation framework which only takes care displaying visaul effect of validation results for each validation field.
    /// Don't use it to control Navigation. View Model should control it. 
    /// </summary>
    public class ValidationContainer : ContentControl, IValidationContainer
    {
        
        /// <summary>
        /// used internally by valiation framework. 
        /// </summary>
        public static readonly DependencyProperty ValidationFieldHandlerProperty =
            DependencyProperty.RegisterAttached("ValidationFieldHandler",
                                                typeof(ValidationFieldHandler),
                                                typeof(ValidationContainer),
                                                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// The name of a property of the IValidationSource. When this IValidationSource notifies this property changed, 
        /// ValidationContainer will validate this field with IValiationSource and set ValidationResult to the binded UI control.
        /// </summary>
        public static readonly DependencyProperty ValidationFieldProperty =
            DependencyProperty.RegisterAttached("ValidationField", typeof(string), typeof(ValidationContainer),
                                                new PropertyMetadata(string.Empty, OnValidateFieldSet));

        /// <summary>
        /// Can be used to display validation result for the binded UI control.
        /// </summary>
        public static readonly DependencyProperty ValidationResultProperty =
            DependencyProperty.RegisterAttached("ValidationResult",
                                                typeof(ValidationResult),
                                                typeof(ValidationContainer),
                                                new PropertyMetadata(null, OnValidationResultSet));



        /// <summary>
        /// An object of IValidationSource which provide vilidation and normally is View Model.
        /// </summary>
        public static readonly DependencyProperty ValidationSourceProperty =
            DependencyProperty.Register("ValidationSource", typeof(IValidationSource),
                                        typeof(ValidationContainer),
                                        new FrameworkPropertyMetadata(null, OnValidationSourcePropertySet));

        public static readonly DependencyProperty ValidationEnabledProperty =
          DependencyProperty.Register("ValidationEnabled", typeof(bool),
                                      typeof(ValidationContainer),
                                      new FrameworkPropertyMetadata(false, OnValidationEnabledSet));

        public static readonly DependencyProperty ValidationTemplateProperty = DependencyProperty.RegisterAttached("ValidationTemplate",
            typeof(ControlTemplate), typeof(ValidationContainer), new PropertyMetadata(null));

        public static ControlTemplate GetValidationTemplate(DependencyObject obj)
        {
            return (ControlTemplate)obj.GetValue(ValidationTemplateProperty);
        }

        public static void SetValidationTemplate(DependencyObject obj, ControlTemplate value)
        {
            obj.SetValue(ValidationTemplateProperty, value);
        }

        internal static readonly DependencyProperty ValidationErrorAdornerProperty = DependencyProperty.RegisterAttached("ValidationErrorAdorner",
            typeof(Adorner), typeof(ValidationContainer), new UIPropertyMetadata(null));


        #region IValidationContainer Members



        //not strong type it as IValidationSource, in order to avoid exception when binding is not ready.
        public IValidationSource ValidationSource
        {
            get { return GetValue(ValidationSourceProperty) as IValidationSource; }
            set
            {
               
                    SetValue(ValidationSourceProperty, value);
                    ValidationSourceChanged.Invoke(this, null);
                
            }
        }

      

        public event EventHandler ValidationSourceChanged = delegate { };
        public event EventHandler ValidationEnabledChanged = delegate { };


        public bool ValidationEnabled
        {
            get { return (bool)GetValue(ValidationEnabledProperty); }
            set
            {
                
                    SetValue(ValidationEnabledProperty, value);
                    ValidationEnabledChanged.Invoke(this, null);
            }
        }

        #endregion



        public static string GetValidationField(DependencyObject d)
        {
            return (string)d.GetValue(ValidationFieldProperty);
        }

        public static void SetValidationField(DependencyObject d, string value)
        {
            d.SetValue(ValidationFieldProperty, value);
        }

        public static ValidationResult GetValidationResult(DependencyObject d)
        {
            return (ValidationResult)d.GetValue(ValidationResultProperty);
        }

        public static void SetValidationResult(DependencyObject d, ValidationResult value)
        {
            d.SetValue(ValidationResultProperty, value);
        }

        private static void OnValidationSourcePropertySet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uiElement = d as IValidationContainer;
            if (uiElement != null)
            {
                uiElement.ValidationSource = e.NewValue as IValidationSource;
                Debug.WriteLine(string.Format("Validation source {0} set on {1}", e.NewValue, uiElement));
            }
        }

        private static void OnValidateFieldSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uiElement = d as FrameworkElement;
            if (uiElement == null) throw new ApplicationException("ValidationField property should be set on a FrameworkElement.");
            if (uiElement.IsLoaded) throw new ApplicationException("ValidationField property should not be set after it is loaded.");
            var filed = e.NewValue as string;

            var oldValidationFieldHandler = uiElement.GetValue(ValidationFieldHandlerProperty) as ValidationFieldHandler;
            if (oldValidationFieldHandler != null)
            {
                oldValidationFieldHandler.UnRegisterEventFromValidationSource();
                uiElement.SetValue(ValidationFieldHandlerProperty, null);
            }

            uiElement.Loaded += (s, arg) => SetValidationHandler(uiElement, filed);
        }

        private static void OnValidationEnabledSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uiElement = d as IValidationContainer;
            if (uiElement != null)
            {
                uiElement.ValidationEnabled = (bool)e.NewValue;
                Debug.WriteLine(string.Format("ValidationEnabled {0} set on {1}", e.NewValue, uiElement));

            }
        }


        private static void SetValidationHandler(FrameworkElement uiElement, string filed)
        {
            var oldValidationFieldHandler = uiElement.GetValue(ValidationFieldHandlerProperty) as ValidationFieldHandler;
            if (oldValidationFieldHandler == null) //only need set once
            {
                IValidationContainer validationContainer = new UiTreeHelper().FindValidationContainer(uiElement);
                var validationFieldHandler = new ValidationFieldHandler(new UIElementWrapper(uiElement), filed, validationContainer);
                uiElement.SetValue(ValidationFieldHandlerProperty, validationFieldHandler);
            }
        }

        private static void OnValidationResultSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uiElement = d as FrameworkElement;
            if (uiElement == null) throw new ApplicationException("ValidationResult property should be set on a FrameworkElement.");
            ReoveAdorner(uiElement);
            var validationResult = e.NewValue as ValidationResult;
            if (validationResult != null)
            {
                if (!validationResult.IsValid)
                    AddAdorner(uiElement, e.NewValue);
            }
        }

        private static void AddAdorner(FrameworkElement element, object dataContext)
        {
            ControlTemplate template = GetValidationTemplate(element);
            if (template == null) return;
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(element);
            if (adornerLayer != null)
            {

                var adorner = new TemplatedAdorner(element, dataContext, template);
                adornerLayer.Add(adorner);
                element.SetValue(ValidationErrorAdornerProperty, adorner);
            }
        }

        private static void ReoveAdorner(FrameworkElement element)
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(element);
            if (adornerLayer != null)
            {
                var oldAdorner = element.GetValue(ValidationErrorAdornerProperty) as Adorner;
                if (oldAdorner != null)
                {
                    adornerLayer.Remove(oldAdorner);
                }

            }
        }
    }
}

