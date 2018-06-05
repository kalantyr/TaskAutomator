using System;
using System.Net;
using System.Windows;
using TaskAutomator.Core;
using TaskAutomator.Tfs2015;
using TfsAutomator.WinUI.Properties;

namespace TfsAutomator.WinUI
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            #region MyRegion

            Settings.Default.UserDomain = "REGION";
            Settings.Default.UserLogin = "VTB198703";
            Settings.Default.UserPassword = "***";

            #endregion

            var credentials = new NetworkCredential(Settings.Default.UserLogin, Settings.Default.UserPassword, Settings.Default.UserDomain);
            ITaskService taskService = new Tfs2015Service(new Uri(Settings.Default.TfsAddress), credentials);
            var pingResult = taskService.Ping();
            if (!string.IsNullOrEmpty(pingResult))
            {
                App.ShowError(new Exception(pingResult));
                return;
            }

            var task = taskService.GetTask("92757");
            task.Description.Equals(null);

            task.Description = task.Description.Replace("..", ".");
            if (taskService.UpdateTask(task).IsSuccess)
            {
            }
        }
    }
}
