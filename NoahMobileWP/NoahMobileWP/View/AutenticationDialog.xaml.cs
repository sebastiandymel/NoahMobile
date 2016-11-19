using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace NoahMobileWP.View
{
    public partial class AutenticationDialog : PhoneApplicationPage
    {
        private AutenticationDialogViewModel vm;
        public AutenticationDialog()
        {
            InitializeComponent();
            this.vm = new AutenticationDialogViewModel();
            DataContext = this.vm;
            this.vm.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ServerAdress")
            {
                if (this.vm.Token != null)
                {
                    NavigationService.GoBack();
                }
            }
        }
    }
}