using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace PageManager.Gui {
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            EventManager.RegisterClassHandler(typeof(TextBoxBase), UIElement.PreviewMouseLeftButtonDownEvent,
               new MouseButtonEventHandler(SelectivelyHandleMouseButton), true);

            EventManager.RegisterClassHandler(typeof(TextBoxBase), UIElement.GotKeyboardFocusEvent,
              new RoutedEventHandler(SelectAllText), true);

            EventManager.RegisterClassHandler(typeof(Window), UIElement.KeyDownEvent,
              new KeyEventHandler(ExitWindow), true);

            base.OnStartup(e);
        }

        private static void SelectivelyHandleMouseButton(object sender, MouseButtonEventArgs e) {
            var textbox = (sender as TextBoxBase);
            if (textbox != null && !textbox.IsKeyboardFocusWithin) {
                e.Handled = true;
                textbox.Focus();
            }
        }

        private static void SelectAllText(object sender, RoutedEventArgs e) {
            if (e.OriginalSource is TextBoxBase textBox) {
                textBox.SelectAll();
            }
        }

        private static void ExitWindow(object sender, KeyEventArgs e) {
            if (e.OriginalSource is Window window && e.Key == Key.Escape) {
                window.Close();
            }
        }
    }
}
