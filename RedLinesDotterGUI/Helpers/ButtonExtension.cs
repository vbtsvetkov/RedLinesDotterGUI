using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedLinesDotterGUI.Extensions
{
    public class ButtonExtension
    {
        public static readonly DependencyProperty IconProperty;

        static ButtonExtension()
        {
            var metadata = new FrameworkPropertyMetadata(null);

            IconProperty = DependencyProperty.RegisterAttached("Icon",
                typeof(FrameworkElement),
                typeof(ButtonExtension));
        }

        public static FrameworkElement GetIcon(DependencyObject obj)
        {
            return (FrameworkElement)obj.GetValue(IconProperty);
        }

        public static void SetIcon(DependencyObject obj, FrameworkElement value)
        {
            obj.SetValue(IconProperty, value);
        }
    }
}
