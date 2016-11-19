using SEDY.PhoneUIToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoahMobileWP.ViewModel
{
    public class ClientDetailsViewModel : ViewModelBase
    {
        private PatientViewModel selectedClient;
        public PatientViewModel SelectedClient
        { 
            get { return this.selectedClient; } 
            set { this.selectedClient = value; RaisePropertyChanged("SelectedClient"); } 
        }
    }
}
