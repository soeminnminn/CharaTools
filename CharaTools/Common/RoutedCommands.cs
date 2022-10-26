using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace CharaTools
{
    public static class RoutedCommands
    {
        public static RoutedCommand ExportJson = new RoutedCommand("ExportJson", typeof(Window));
        public static RoutedCommand Exit = new RoutedCommand("Exit", typeof(Window));
        public static RoutedCommand Options = new RoutedCommand("Options", typeof(Window));
        public static RoutedCommand About = new RoutedCommand("About", typeof(Window));

        public static void RaiseClickEvent(this Control sender, ExecutedRoutedEventArgs e)
        {
            if (sender is MenuItem)
            {
                (sender as MenuItem)?.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
            }
            else if (sender is ButtonBase)
            {
                (sender as ButtonBase)?.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }
    }
}
