using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace VisualValidation
{
    /// <summary>
    /// Provides an implementation of the WPF AdornedElementPlaceholder that can be used in scenarios outside of WPF's internal
    /// validation mechanisms (the out-of-the-box AdornedElementPlaceholder relies on a TemplatedAdorner object which is an internal
    /// class).
    /// </summary>
    [ContentProperty("Child")]
    public class AdornedElementPlaceholderExtended : FrameworkElement, IAddChild
    {
        private UIElement _child;
        private TemplatedAdorner _templatedAdorner;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdornedElementPlaceholderExtended"/> class.
        /// </summary>
        public AdornedElementPlaceholderExtended()
        {
            
        }

        /// <summary>
        /// Gets the adorned element.
        /// </summary>
        /// <value>The adorned element.</value>
        public UIElement AdornedElement
        {
            get
            {
                if (TemplatedAdorner != null)
                {
                    return TemplatedAdorner.AdornedElement;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the child.
        /// </summary>
        /// <value>The child.</value>
        public UIElement Child
        {
            get { return _child; }
            set
            {
                var child = _child;
                if (child != value)
                {
                    RemoveVisualChild(child);
                    RemoveLogicalChild(child);
                    _child = value;
                    AddVisualChild(value);
                    AddLogicalChild(value);
                    InvalidateMeasure();
                }
            }
        }

        private TemplatedAdorner TemplatedAdorner
        {
            get
            {
                if (_templatedAdorner == null)
                {
                    var templatedParent = TemplatedParent as FrameworkElement;
                    if (templatedParent != null)
                    {
                        _templatedAdorner = VisualTreeHelper.GetParent(templatedParent) as TemplatedAdorner;
                        if ((_templatedAdorner != null) && (_templatedAdorner.PlaceholderReferenceElement == null))
                        {
                            _templatedAdorner.PlaceholderReferenceElement = this;
                        }
                    }
                }
                return _templatedAdorner;
            }
        }

        /// <summary>
        /// Gets the number of visual child elements within this element.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The number of visual child elements for this element.
        /// </returns>
        protected override int VisualChildrenCount
        {
            get
            {
                if (_child != null)
                {
                    return 1;
                }
                return 0;
            }
        }

        /// <summary>
        /// Adds a child object.
        /// </summary>
        /// <param name="value">The child object to add.</param>
        public void AddChild(object value)
        {
            if (value != null)
            {
                if (!(value is UIElement))
                {
                }
                if (Child != null)
                {
                }
                Child = (UIElement) value;
            }
        }

        /// <summary>
        /// Adds the text content of a node to the object.
        /// </summary>
        /// <param name="text">The text to add to the object.</param>
        public void AddText(string text)
        {
        }

        /// <summary>
        /// Arranges the override.
        /// </summary>
        /// <param name="arrangeBounds">The arrange bounds.</param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            var child = Child;
            if (child != null)
            {
                child.Arrange(new Rect(arrangeBounds));
            }
            return arrangeBounds;
        }

        /// <summary>
        /// Overrides <see cref="M:System.Windows.Media.Visual.GetVisualChild(System.Int32)"/>, and returns a child at the specified index from a collection of child elements.
        /// </summary>
        /// <param name="index">The zero-based index of the requested child element in the collection.</param>
        /// <returns>
        /// The requested child element. This should not return null; if the provided index is out of range, an exception is thrown.
        /// </returns>
        protected override Visual GetVisualChild(int index)
        {
            if ((_child == null) || (index != 0))
            {
                throw new ArgumentOutOfRangeException();
            }
            return _child;
        }

        /// <summary>
        /// Measures the override.
        /// </summary>
        /// <param name="constraint">The constraint.</param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size constraint)
        {
            if (TemplatedParent == null)
            {
                throw new Exception("TemplatedParent cannot be null.");
            }
            if (AdornedElement == null)
            {
                return new Size(0.0, 0.0);
            }
            Size renderSize = AdornedElement.RenderSize;
            UIElement child = Child;
            if (child != null)
            {
                child.Measure(renderSize);
            }
            return renderSize;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.FrameworkElement.Initialized"/> event. This method is invoked whenever <see cref="P:System.Windows.FrameworkElement.IsInitialized"/> is set to true internally.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs"/> that contains the event data.</param>
        protected override void OnInitialized(EventArgs e)
        {
            if (base.TemplatedParent == null)
            {
            }
            base.OnInitialized(e);
        }
    }
}