using Himsa.Noah.MobileAccessLayer;
using SEDY.PhoneUIToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoahMobileWP.View
{
    public class AutenticationDialogViewModel: ViewModelBase
    {
        public AutenticationDialogViewModel()
        {
            NoahApiWrapper.NoahWrapper.Instance.AuthorizationRequested += OnAuthorize;
            NoahApiWrapper.NoahWrapper.Instance.ConnectionError += OnConnectionError;
            NoahApiWrapper.NoahWrapper.Instance.Connect();
        }

        private void OnConnectionError(object sender, NoahApiWrapper.ConnectionErrorEventArgs e)
        {
            ErrorOccured = true;
            if (e.Exception != null)
            {
                ErrorMessage = e.Exception.Message;
            }
        }

        private void OnAuthorize(object sender, NoahApiWrapper.AuthorizizationEventArgs e)
        {
            ServerAdress = e.ServerAdress;
        }

        private Uri serverAdress;
        public Uri ServerAdress
        {
            get { return this.serverAdress; }
            set
            {
                this.serverAdress = value;

                if (value != null)
                {
                    var token = Token.FromUri(value);
                    if (token != null)
                    {
                        Token = token;
                    }
                }
                
                RaisePropertyChanged("ServerAdress");                
            }
        }

        public Token Token { get; private set; }

        private bool errorOccured;
        public bool ErrorOccured
        {
            get { return this.errorOccured; }
            set { this.errorOccured = value; RaisePropertyChanged("ErrorOccured"); }
        }

        private string errormsg;
        public string ErrorMessage
        {
            get { return this.errormsg; }
            set { this.errormsg = value; RaisePropertyChanged("ErrorMessage"); }
        }
    }
}
