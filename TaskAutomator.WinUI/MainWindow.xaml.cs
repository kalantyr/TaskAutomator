﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
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
            Settings.Default.UserDomain = _domain.Text;
            Settings.Default.UserLogin = _login.Text;
            Settings.Default.UserPassword = _password.Password;

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

            var getTaskResult = taskService.GetTask("92757");
            if (!getTaskResult.IsSuccess)
            {
                App.ShowError(getTaskResult.Exception);
                return;
            }

            var task = getTaskResult.Data;
            task.Description.Equals(null);

            task.Description = task.Description.Replace("..", ".");
            if (taskService.UpdateTask(task).IsSuccess)
            {
            }
        }

        private void OnJobClick(object sender, RoutedEventArgs e)
        {
            Settings.Default.UserDomain = _domain.Text;
            Settings.Default.UserLogin = _login.Text;
            Settings.Default.UserPassword = _password.Password;

            var credentials = new NetworkCredential(Settings.Default.UserLogin, Settings.Default.UserPassword, Settings.Default.UserDomain);

            var taskSelector = new SimpleTaskSelector(new [] { "1", "2", "3" });
            var processor = new LinkReplacer(new Tfs2015Service(new Uri(Settings.Default.TfsAddress), credentials));
            var job = new Job(taskSelector, processor);
            var jobResult = job.Run();
            if (!jobResult.IsSuccess)
            {
                App.ShowError(new Exception(jobResult.Error));
                return;
            }
            else
                Settings.Default.Save();
        }

        private void OnCopyLinkButtonClick(object sender, RoutedEventArgs e)
        {
            Settings.Default.UserDomain = _domain.Text;
            Settings.Default.UserLogin = _login.Text;
            Settings.Default.UserPassword = _password.Password;

            var credentials = new NetworkCredential(Settings.Default.UserLogin, Settings.Default.UserPassword, Settings.Default.UserDomain);

            var result = LinkReplacer.IdToLink(_taskIdTextBox.Text, new Tfs2015Service(new Uri(Settings.Default.TfsAddress), credentials), new Uri(Settings.Default.TfsAddress));
            if (result.IsSuccess)
            {
                Clipboard.SetText(result.Data);
                Settings.Default.Save();
            }
            else
                App.ShowError(result.Exception);
        }

        private void OnTaskIdTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Handled)
                return;

            if (e.Key == Key.Enter)
            {
                OnCopyLinkButtonClick(sender, new RoutedEventArgs());
                e.Handled = true;
            }
        }
    }
}
