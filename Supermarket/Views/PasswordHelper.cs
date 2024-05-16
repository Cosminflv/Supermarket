using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Supermarket.Views
{
    public static class PasswordHelper
    {
        public static readonly DependencyProperty BoundPasswordProperty =
        DependencyProperty.RegisterAttached("BoundPassword",
                                            typeof(string),
                                            typeof(PasswordHelper),
                                            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static string GetBoundPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(BoundPasswordProperty);
        }

        public static void SetBoundPassword(DependencyObject obj, string value)
        {
            obj.SetValue(BoundPasswordProperty, value);
        }

        public static readonly DependencyProperty BindPasswordProperty =
            DependencyProperty.RegisterAttached("BindPassword",
                                                typeof(bool),
                                                typeof(PasswordHelper),
                                                new PropertyMetadata(false, OnBindPasswordChanged));

        private static void OnBindPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = d as PasswordBox;
            if (passwordBox == null)
            {
                return;
            }

            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            SetBoundPassword(passwordBox, passwordBox.Password);
        }

        public static bool GetBindPassword(DependencyObject obj)
        {
            return (bool)obj.GetValue(BindPasswordProperty);
        }

        public static void SetBindPassword(DependencyObject obj, bool value)
        {
            obj.SetValue(BindPasswordProperty, value);
        }
    }
}
