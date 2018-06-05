﻿using System;
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

            _domain.Text = Settings.Default.UserDomain;
            _login.Text = Settings.Default.UserLogin;
            _password.Password = Settings.Default.UserPassword;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            #region MyRegion

            Settings.Default.UserDomain = _domain.Text;
            Settings.Default.UserLogin = _login.Text;
            Settings.Default.UserPassword = _password.Password;

            #endregion

            var credentials = new NetworkCredential(Settings.Default.UserLogin, Settings.Default.UserPassword, Settings.Default.UserDomain);
            ITaskService taskService = new Tfs2015Service(new Uri(Settings.Default.TfsAddress), credentials);
            var pingResult = taskService.Ping();
            if (!string.IsNullOrEmpty(pingResult))
            {
                App.ShowError(new Exception(pingResult));
                return;
            }
            else
                Settings.Default.Save();

            var task = taskService.GetTask("92757");
            task.Description.Equals(null);

            task.Description = task.Description.Replace("..", ".");
            if (taskService.UpdateTask(task).IsSuccess)
            {
            }
        }
    }
}
