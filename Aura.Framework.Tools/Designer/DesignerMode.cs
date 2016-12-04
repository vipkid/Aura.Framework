using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Aura.Framework.Tools.Designer
{
    /// <summary>
    /// Represents the designer tool, used to hide elements in the designer.
    /// </summary>
    public static class DesignerMode
    {
        #region Dependency Properties

        /// <summary>
        /// Returns a <see cref="bool"/> value whether the <see cref="FrameworkElement"/> is hidden or not.
        /// </summary>
        public static readonly DependencyProperty IsHiddenProperty = DependencyProperty.RegisterAttached("IsHidden", typeof(bool), typeof(DesignerMode), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsHiddenChanged)));

        #endregion Dependency Properties

        #region Methods

        /// <summary>
        /// Sets the <see cref="FrameworkElement"/> as hidden.
        /// </summary>
        public static void SetIsHidden(FrameworkElement element, bool value)
        {
            element.SetValue(IsHiddenProperty, value);
        }

        /// <summary>
        /// Returns a <see cref="bool"/> value whether the <see cref="FrameworkElement"/> is hidden or not.
        /// </summary>
        public static bool GetIsHidden(FrameworkElement element)
        {
            return (bool)element.GetValue(IsHiddenProperty);
        }

        #endregion Methods

        #region Private Methods

        /// <summary>
        /// Occurs when the hidden-state is changed.
        /// </summary>
        private static void OnIsHiddenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(d)) return;
            var element = (FrameworkElement)d;
            element.RenderTransform = (bool)e.NewValue
               ? new ScaleTransform(0, 0)
               : null;
        }

        #endregion Private Methods
    }
}