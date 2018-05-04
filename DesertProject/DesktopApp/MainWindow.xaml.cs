﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static Application GetApp()
        {
            return Application.Current;
        }
        private void MenuExit_OnClick(object sender, RoutedEventArgs e) => Close();

        private void MenuRestart_OnClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void PlayerSettings_OnClick(object sender, RoutedEventArgs e) => new Settings().Show();

        private void Pause_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Play_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void About_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MainSettings_OnClick(object sender, RoutedEventArgs e)=>new MainSetings().Show();
    }
}
