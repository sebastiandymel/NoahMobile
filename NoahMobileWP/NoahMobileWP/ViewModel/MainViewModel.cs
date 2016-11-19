using NoahApiWrapper;
using SEDY.PhoneUIToolkit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NoahMobileWP.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private bool isConnected;
        
        public MainViewModel()
        {
            LoadPatientDataCommand = new RelayCommand(LoadPatientData, CanLoadPatientData);
            NoahWrapper.Instance.Connected += OnConnected;
            Patients = new ObservableCollection<PatientViewModel>();
            NoahWrapper.Instance.UseMockedData = false;
            this.isConnected = NoahApiWrapper.NoahWrapper.Instance.IsConnected;
        }

        public RelayCommand LoadPatientDataCommand { get; private set; }
        public ObservableCollection<PatientViewModel> Patients { get; private set; }

        #region Private helpers

        private bool CanLoadPatientData()
        {
            return this.isConnected;
        }

        private void LoadPatientData()
        {
            var data = NoahWrapper.Instance.GetPatients();
            foreach (var item in data)
            {
                var vm = new PatientViewModel();
                vm.Name = item.Name;
                vm.Lastname = item.Lastname;
                vm.Id = item.Id;
                Patients.Add(vm);
            }
        }

        private void OnConnected(object sender, EventArgs e)
        {
            this.isConnected = NoahApiWrapper.NoahWrapper.Instance.IsConnected;
            LoadPatientDataCommand.RaiseCanExecuteChanged();
        }

        #endregion Private helpers
    }

    public class PatientViewModel : ViewModelBase
    {
        private string id;
        public string Id
        {
            get { return this.id; }
            set
            {
                this.id = value;
                RaisePropertyChanged("Id");
            }
        }


        private string name;
        public string Name
        {
            get { return this.name; }
            set 
            { 
                this.name = value;
                RaisePropertyChanged("Name");
            }
        }

        private string lastname;
        public string Lastname
        {
            get { return this.lastname; }
            set
            {
                this.lastname = value;
                RaisePropertyChanged("Lastname");
            }
        }
    }
}
