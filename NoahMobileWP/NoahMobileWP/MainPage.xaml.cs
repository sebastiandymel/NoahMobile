using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NoahMobileWP.Resources;
using SEDY.PhoneUIToolkit;
using NoahMobileWP.ViewModel;

namespace NoahMobileWP
{
    public partial class MainPage : PhoneApplicationPage
    {
        private MainViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();
            this.viewModel = App.AppViewModel;
            DataContext = this.viewModel;
        }

        private void OnConnect(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/AutenticationDialog.xaml", UriKind.Relative));
        }
    }
}