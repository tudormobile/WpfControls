using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Tudormobile.Wpf.Controls
{
    /// <summary>
    /// A Toggle Switch is a checkbox/togglebutton that uses and animated switch to change states. The 'Content' property
    /// is used to display the 'Off' state content, which can be (null). The 'OnContent' property can be used to represent
    /// the 'On' state. The default values for the content properties are 'Off' and 'On'. If the 'On' content does not
    /// exist, then the Content property is used for all states.
    /// </summary>
    public class ToggleSwitch : ToggleButton
    {
        private UIElement? _container;
        private Thumb? _thumb;
        private double _startX;

        static ToggleSwitch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleSwitch), new FrameworkPropertyMetadata(typeof(ToggleSwitch)));
        }

        /// <summary>
        /// True if there is 'OnContent' present.
        /// </summary>
        public bool HasOnContent
        {
            get { return (bool)GetValue(HasOnContentProperty); }
            set { SetValue(HasOnContentProperty, value); }
        }

        /// <inheritdoc/>
        public static readonly DependencyProperty HasOnContentProperty = DependencyProperty.Register(
            "HasOnContent",
            typeof(bool),
            typeof(ToggleSwitch),
            new PropertyMetadata(false));

        /// <summary>
        /// Content for the 'On' state of the switch
        /// </summary>
        public object? OnContent
        {
            get { return (object)GetValue(OnContentProperty); }
            set { SetValue(OnContentProperty, value); }
        }

        ///  <inheritdoc/>
        public static readonly DependencyProperty OnContentProperty = DependencyProperty.Register(
            "OnContent",
            typeof(object),
            typeof(ToggleSwitch),
            new PropertyMetadata(null, setHasOnContent));

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            _container = Template.FindName("PART_Container", this) as UIElement;
            _thumb = Template.FindName("PART_Thumb", this) as Thumb;
            if (_thumb != null && _container != null)
            {
                _thumb.DragCompleted += thumb_DragCompleted;
                _thumb.DragStarted += thumb_DragStarted;
                _thumb.DragDelta += thumb_DragDelta;
            }
        }

        /// <inheritdoc/>
        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);
            if (_thumb != null) Canvas.SetLeft(_thumb, 22);
        }

        /// <inheritdoc/>
        protected override void OnUnchecked(RoutedEventArgs e)
        {
            base.OnUnchecked(e);
            if (_thumb != null) Canvas.SetLeft(_thumb, 4);
        }

        private void thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb thumb)
            {
                var x = Math.Clamp(Mouse.GetPosition(_container).X, 11, 29);
                Canvas.SetLeft(thumb, x - 7);
            }
        }

        private void thumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (sender is Thumb thumb)
            {
                _startX = Canvas.GetLeft(thumb);
            }
        }

        private void thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (sender is Thumb thumb)
            {
                if (e.HorizontalChange > 10) { Canvas.SetLeft(thumb, 22); IsChecked = true; }
                else if (e.HorizontalChange < -10) { Canvas.SetLeft(thumb, 4); IsChecked = false; }
                else Canvas.SetLeft(thumb, _startX);
            }
        }

        private static void setHasOnContent(DependencyObject dObj, DependencyPropertyChangedEventArgs args)
        {
            if (dObj is ToggleSwitch toggle)
            {
                toggle.HasOnContent = args.NewValue != null;
            }
        }
    }
}
