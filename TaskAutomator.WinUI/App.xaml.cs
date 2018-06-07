using System;
using System.Windows;

namespace TfsAutomator.WinUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        internal static void ShowError(Exception error)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));
            MessageBox.Show(error.GetBaseException().Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
