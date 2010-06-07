using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace VisualValidation
{
    /// <summary>
    /// Provides an implementation of the WPF TemplatedAdorner class which is not internal.
    /// </summary>
    public sealed class TemplatedAdorner : Adorner
    {
        private readonly Control _child;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplatedAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement">The adorned element.</param>
        /// <param name="dataContext">The data context.</param>
        /// <param name="adornerTemplate">The adorner template.</param>
        public TemplatedAdorner(UIElement adornedElement, object dataContext, ControlTemplate adornerTemplate)
            : base(adornedElement)
        {
            _child = new Control {Template = adornerTemplate};
            DataContext = dataContext;
            AddVisualChild(_child);
        }

        /// <summary>
        /// Gets or sets the reference element.
        /// </summary>
        /// <value>The reference element.</value>
        public FrameworkElement PlaceholderReferenceElement { get; set; }

        /// <summary>
        /// Gets the number of visual child elements within this element.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The number of visual child elements for this element.
        /// </returns>
        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        protected override Size ArrangeOverride(Size size)
        {
            Size baseSize = base.ArrangeOverride(size);
            if (_child != null)
            {
                _child.Arrange(new Rect(new Point(), baseSize));
            }
            return baseSize;
        }

        /// <summary>
        /// Returns a <see cref="T:System.Windows.Media.Transform"/> for the adorner, based on the transform that is currently applied to the adorned element.
        /// </summary>
        /// <param name="transform">The transform that is currently applied to the adorned element.</param>
        /// <returns>A transform to apply to the adorner.</returns>
        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            if (PlaceholderReferenceElement == null)
            {
                return transform;
            }
            var group = new GeneralTransformGroup();
            group.Children.Add(transform);
            var transform2 = TransformToDescendant(PlaceholderReferenceElement);
            if (transform2 != null)
            {
                group.Children.Add(transform2);
            }
            return group;
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
            return _child;
        }

        /// <summary>
        /// Implements any custom measuring behavior for the adorner.
        /// </summary>
        /// <param name="constraint">A size to constrain the adorner to.</param>
        /// <returns>
        /// A <see cref="T:System.Windows.Size"/> object representing the amount of layout space needed by the adorner.
        /// </returns>
        protected override Size MeasureOverride(Size constraint)
        {
            if (PlaceholderReferenceElement != null && AdornedElement != null && AdornedElement.IsMeasureValid && !AreClose(PlaceholderReferenceElement.DesiredSize, AdornedElement.DesiredSize))
            {
                PlaceholderReferenceElement.InvalidateMeasure();
            }
            _child.Measure(constraint);
            return _child.DesiredSize;
        }

        private static bool AreClose(double value1, double value2)
        {
            if (value1 == value2)
            {
                return true;
            }
            var num = ((Math.Abs(value1) + Math.Abs(value2)) + 10.0)*2.2204460492503131E-16;
            var num2 = value1 - value2;
            return ((-num < num2) && (num > num2));
        }

        private static bool AreClose(Size size1, Size size2)
        {
            return (AreClose(size1.Width, size2.Width) && AreClose(size1.Height, size2.Height));
        }
    }
}