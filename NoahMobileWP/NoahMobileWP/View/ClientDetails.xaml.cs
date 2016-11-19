using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NoahMobileWP.ViewModel;

namespace NoahMobileWP.View
{
    public partial class ClientDetails : PhoneApplicationPage
    {
        private ClientDetailsViewModel viewModel;

        public ClientDetails()
        {
            InitializeComponent();
            this.viewModel = new ClientDetailsViewModel();
            DataContext = this.viewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string clientId = string.Empty;

            if (NavigationContext.QueryString.TryGetValue("clientId", out clientId))
            {
                var client = App.AppViewModel.Patients.FirstOrDefault(p => p.Id == clientId);
                this.viewModel.SelectedClient = client;
            }                
        }
    }    
}