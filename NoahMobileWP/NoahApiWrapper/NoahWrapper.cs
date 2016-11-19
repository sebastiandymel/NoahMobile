using System;
using System.Collections.Generic;
using Himsa.Noah.MobileAccessLayer;
using System.Diagnostics;

namespace NoahApiWrapper
{
    public class NoahWrapper
    {
        private const int DemoAppModuleId = 65957;
        private AccessLayer accessLayer;
        private static readonly Lazy<NoahWrapper> instance = new Lazy<NoahWrapper>(() => new NoahWrapper());
        private bool isConnected;
        private ServerInfo connectedServer;
        private Token token;
        private Configuration configuration;
        private bool isConnecting;

        public NoahWrapper()
        {

        }

        #region Public members

        public static NoahWrapper Instance { get { return instance.Value; } }

        public bool UseMockedData { get; set; }
        public event EventHandler Connected = delegate { };
        public event EventHandler Disconnected = delegate { };
        public event EventHandler<ConnectionErrorEventArgs> ConnectionError = delegate { };
        public event EventHandler<AuthorizizationEventArgs> AuthorizationRequested = delegate { };

        public void Connect()
        {
            if (this.isConnected || this.isConnecting)
            {
                return;
            }

            try
            {
                this.isConnecting = true;
                //
                // LOGIN TO NOAH SERVER
                //
                this.configuration = new Configuration();
                this.connectedServer = ConnectionHelper.GetServer(configuration.Alias);
                
                var uri = ConnectionHelper.GetAuthenticationUri(configuration, this.connectedServer);
                AuthorizationRequested(this, new AuthorizizationEventArgs { ServerAdress = uri });
                // at this point token should be established                
            }
            catch (Exception ex)
            {
                this.isConnecting = false;
                Debug.WriteLine(ex.Message);
                ConnectionError(this, new ConnectionErrorEventArgs { Exception = ex });
            }
        }

        public void SetToken(Token newToken)
        {
            this.token = newToken;
            
            // 
            // ACCESS DATA FROM NOAH
            //
            this.accessLayer = new AccessLayer(
                ConnectionHelper.ResourceServer,
                this.connectedServer.RemoteHostId.ToString(),
                ConnectionHelper.DemoAppClientId,
                DemoAppModuleId,
                token);
            var registration = this.accessLayer.RegisterApp(this.configuration.Alias, "0.1", "DemoApp");

            //
            // SUCCESSFULL CONNECTION
            //
            this.isConnected = true;
            this.isConnecting = false;
            Connected(this, EventArgs.Empty);
        }

        public void Disconnect()
        {
            if (!this.isConnected)
            {
                return;
            }
            this.isConnected = false;
            Disconnected(this, EventArgs.Empty);
        }

        public PatientData[] GetPatients(int count = 100)
        {
            if (UseMockedData)
            {
                return GenerateMockedData();
            }

            var result = new List<PatientData>();
            return result.ToArray();
        }

        public bool IsConnected { get { return this.isConnected; } }

        #endregion Public members

        #region Private helpers

        private PatientData[] GenerateMockedData()
        {
            var result = new List<PatientData>();

            var date = new DateTime(1984, 05, 20);
            for (int i = 0; i < 100; i++)
            {

                result.Add(new PatientData()
                {
                    Id = i.ToString(),
                    Name = "Sebastian",
                    Lastname = "Dymel",
                    Birthdate = date
                });
            }
            return result.ToArray();
        }

        #endregion Private helpers

    }

    #region Additional types exposed by NoahWrapper

    public class PatientData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthdate { get; set; }
    }

    public class Configuration
    {
        public Configuration()
        {
            Alias = "SEDYLAPTOP";
            RemoteHostId = "045d4c23-110d-4d78-b997-7796ad5b48ea";
            LocalIp = "255.255.255.255";
            Token = "dyMtuu0V2X1X0eI24OwNLNGmDSwgfmmi2mCZtWb8PQ7LAvLG5RfV8JS0MVYkUQMH3FMIrwJ9Te0tpONzhJJzq3Jz7zWgmuX21J9wnf18bBn1pv28hui8Owxx8B4kl1j_315STPiXMIkBppXdvGgXDpbgtdE9zFckdapw-8Jl4LTClR6prs5e5EmDerF7oQ3TLPi11whHHOdRwPa7qfIO5GayDTDmae2f_eHXGCV7QqfhVR-5BZ8DckAvKWCJbqBaO5SFjJSlZ9qHUqCHD9hmH5o3uOcH-KncYaRcWYwCch_kGX8Ww5FCv_Izti9ONlC0AIzlkeTX8C8lwtKlMw7Rwj0-OOyjjuJh_OrpbKz0ZxKnf7_K6kWR0K6wdE9b_Y0o";
        }

        public string RemoteHostId { get; set; }
        public string Alias { get; set; }
        public string LocalIp { get; set; }
        public string Token { get; set; }
    }

    public class ConnectionErrorEventArgs : EventArgs
    {
        public Exception Exception { get; set; }
    }

    public class AuthorizizationEventArgs : EventArgs
    {
        public Uri ServerAdress { get; set; }
    }

    #endregion Additional types exposed by NoahWrapper
}
