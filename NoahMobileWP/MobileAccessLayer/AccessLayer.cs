using System;
using System.Collections.Generic;
using System.Threading;

namespace Himsa.Noah.MobileAccessLayer
{
    /// <summary>
    /// Wraps properties and methods for accessing Noah Mobile.
    /// </summary>
    public class AccessLayer
    {
        #region Public_Properties

        /// <summary>
        /// Gets or sets the resource server base address.
        /// </summary>
        /// <value>
        /// The resource server base address.
        /// </value>
        public Uri ResourceServerBaseAddress { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public Token Token { get; set; }

        /// <summary>
        /// Gets or sets the remote host identifier.
        /// </summary>
        /// <value>
        /// The remote host identifier.
        /// </value>
        public string RemoteHostId { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the device identifier.
        /// </summary>
        /// <value>
        /// The device identifier.
        /// </value>
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the module identifier.
        /// </summary>
        /// <value>
        /// The module identifier.
        /// </value>
        public int ModuleId { get; set; }

        private TimeSpan? _timeout;

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>
        /// The timeout.
        /// </value>
        public TimeSpan Timeout
        {
            get { return _timeout ?? (_timeout = WebApi.DefaultTimeout).Value; }
            set { _timeout = value; }
        }

        #endregion

        #region Ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessLayer"/> class.
        /// </summary>
        /// <param name="resourceServerBaseAddress">The resource server base address.</param>
        /// <param name="remoteHostId">The remote host identifier.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="moduleId">The module identifier.</param>
        /// <param name="token">The token.</param>
        /// <param name="deviceId">The device identifier. Optional. If left out, systems MAC address is used.</param>
        public AccessLayer(Uri resourceServerBaseAddress, string remoteHostId, string clientId, int moduleId, Token token, string deviceId = null)
        {
            Log.DebugFormat(
                "Himsa.Noah.MobileAccessLayer.AccessLayer::AccessLayer: resourceServerBaseAddress={0} remoteHostId={1} clientId={2} moduleId={3} token={4} deviceId={5}",
                resourceServerBaseAddress, remoteHostId, clientId, moduleId, token, deviceId);
            ResourceServerBaseAddress = resourceServerBaseAddress;
            RemoteHostId = remoteHostId;
            ClientId = clientId;
            DeviceId = deviceId;
            ModuleId = moduleId;
            Token = token;
        }

        #endregion

        #region Discovery

        

        

        /// <summary>
        /// This method is used to aquire the Noah servers Remote Host Id and/or Local Host Ip Address.
        /// </summary>
        /// <param name="resourceServerBaseAddress">The base url for the OAuth authentication site in the cloud.</param>
        /// <param name="friendlyName">Friendly name. This is the Noah Mobile Alias, found in the Noah Setup.</param>
        /// <param name="connectionpreference">Preferred connection (remote, local or both).</param>
        /// <param name="timeout">Timeout. Optional. If left out, systems deafult timeout is used.</param>
        /// <returns>Returns a <see cref="DiscoveryResponse"/>.</returns>
        /// <remarks>
        /// 	<list type="bullet">
        /// 		<item>
        /// 			<description>TThrows exception in case of error.</description>
        /// 		</item>
        /// 	</list>
        /// </remarks>
        public static DiscoveryResponse Discovery(Uri resourceServerBaseAddress, string friendlyName, ref ConnectionPreferenceType connectionpreference, string ipAddress,TimeSpan? timeout = null)
        {
            try
            {
                
                return WebApi.Discovery(resourceServerBaseAddress, friendlyName, ref connectionpreference, ipAddress, timeout ?? WebApi.DefaultTimeout);               
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::Discovery: {0}", e);
                throw;
            }
        }

        #endregion

        #region Login

        /// <summary>
        /// Gets the implicit grant authentication URI. 
        /// To be used when authenticating on the authentication server.
        /// </summary>
        /// <param name="authenticationServerBaseAddress">The authentication server base address.</param>
        /// <param name="path">The path.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="remoteHostId">The remote host identifier.</param>
        /// <param name="deviceId">The device identifier.</param>
        /// <returns>An uri for the auth server.</returns>
        public static Uri GetImplicitGrantAuthenticationUri(Uri authenticationServerBaseAddress, string path, string clientId, string remoteHostId, string deviceId = null)
        {
            try
            {
                var uri = new Uri(authenticationServerBaseAddress + path);
                uri = Helpers.AppendQuery(uri, "client_id", clientId);
                uri = Helpers.AppendQuery(uri, "remotehostid", remoteHostId);
                uri = Helpers.AppendQuery(uri, "response_type", "token");
                uri = Helpers.AppendQuery(uri, "deviceid", deviceId);
                Log.InfoFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::GetImplicitGrantAuthenticationUri: uri={0}", uri);
                return uri;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::GetImplicitGrantAuthenticationUri: {0}", e);
                throw;
            }
        }


        /// <summary>
        /// Gets the implicit grant authentication URI. 
        /// To be used when authenticating on the authentication server.
        /// </summary>
        /// <param name="authenticationServerBaseAddress">The authentication server base address.</param>
        /// <param name="path">The path.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="remoteHostId">The remote host identifier.</param>
        /// <param name="deviceId">The device identifier.</param>
        /// <returns>An uri for the auth server.</returns>
        public static Uri GetImplicitGrantAuthenticationUri(Uri authenticationServerBaseAddress, string path, string clientId, string remoteHostId, string deviceId = null, string remoteHostName = null)
        {
            try
            {
                var uri = new Uri(authenticationServerBaseAddress + path);
                uri = Helpers.AppendQuery(uri, "client_id", clientId);
                uri = Helpers.AppendQuery(uri, "remotehostid", remoteHostId);
                uri = Helpers.AppendQuery(uri, "response_type", "token");
                uri = Helpers.AppendQuery(uri, "deviceid", deviceId);
                uri = Helpers.AppendQuery(uri, "remotehostname", remoteHostName);
                Log.InfoFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::GetImplicitGrantAuthenticationUri: uri={0}", uri);
                return uri;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::GetImplicitGrantAuthenticationUri: {0}", e);
                throw;
            }
        }


        #endregion

        #region Registration

        /// <summary>
        /// Registers the application.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="version">The version.</param>
        /// <returns>Returns an <see cref="AppRegInfo"/>.</returns>
        public AppRegInfo RegisterApp(string name, string version, string appType)
        {
            try
            {
                

                var appRegInfo = new AppRegInfo { ModuleId = ModuleId, Name = name, Version = version,MobileAppType = appType};
                return WebApi.RegisterApp(ResourceServerBaseAddress, appRegInfo, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::RegisterApp: {0}", e);
                throw;
            }
        }

        /// <summary>
        /// Unregisters the application.
        /// </summary>
        public int UnregisterApp()
        {
            try
            {
                return WebApi.UnregisterApp(ResourceServerBaseAddress, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::UnregisterApp: {0}", e);
                throw;
            }
        }

        #endregion

        #region Permission

        /// <summary>
        /// Gets the permissions for the app, regarding the <see cref="Patient"/> objects.
        /// </summary>
        public List<AppPermission> GetAppPermissions()
        {
            try
            {
                return WebApi.GetAppPermissions(ResourceServerBaseAddress, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::GetAppPermissions: {0}", e);
                throw;
            }
        }

        #endregion

        #region NoahServerSettings

        /// <summary>
        /// Gets the permissions for the app, regarding the <see cref="Patient"/> objects.
        /// </summary>
        public NoahServerSettings GetNoahServerSettings()
        {
            try
            {
                return WebApi.GetNoahServerSettings(ResourceServerBaseAddress, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::GetNoahServerSettings: {0}", e);
                throw;
            }
        }

        #endregion

        #region HIPAALog

        /// <summary>
        /// Add a HIPAA log entry in the connected Noah Server.
        /// </summary>
        /// <param name="logEntry">The log string.</param>
        public void AddAuditTrailEntry(string logEntry)
        {
            try
            {
                WebApi.AddAuditTrailEntry(ResourceServerBaseAddress, logEntry, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::HippaLogEntry: {0}", e);
                throw;
            }
        }

        #endregion
        
        #region Patient

        /// <summary>
        /// Creates the patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns>A copy of the created <see cref="Patient"/></returns>
        public Patient CreatePatient(Patient patient)
        {
            try
            {
                return WebApi.CreatePatient(ResourceServerBaseAddress, patient, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::CreatePatient: {0}", e);
                throw;
            }
        }

        /// <summary>
        /// Updates the patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns>A copy of the updated <see cref="Patient"/></returns>
        public Patient UpdatePatient(Patient patient)
        {
            try
            {
                return WebApi.UpdatePatient(ResourceServerBaseAddress, patient, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::UpdatePatient: {0}", e);
                throw;
            }
        }

        /// <summary>
        /// Gets the patient.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <returns>The <see cref="Patient"/></returns>
        public Patient GetPatient(int patientId)
        {
            try
            {
                return WebApi.GetPatient(ResourceServerBaseAddress, patientId, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::GetPatient: {0}", e);
                throw;
            }
        }

        /// <summary>
        /// Gets the patients.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="page">The requested page number (starting from 1).</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>A list of <see cref="Patient"/>s. There is an upper limit on the number of patients that can be returned.</returns>
        public List<Patient> GetPatients(string searchText, int? page, int? pageSize)
        {
            try
            {
                PatientSearch patientSearch = WebApi.GetPatients(ResourceServerBaseAddress, searchText, page, pageSize,
                    Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
                
                return patientSearch.Patients;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::GetPatients: {0}", e);
                throw;
            }
        }

   
        /// <summary>
        /// Gets the patients.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns>A list of <see cref="Patient"/>s. There is an upper limit on the number of patients that can be returned.</returns>
        public List<Patient> GetPatients(string searchText)
        {
            return GetPatients(searchText, null, null);
        }

        /// <summary>
        /// Gets a list of updated patients based on a DateTime interval.
        /// </summary>
        /// <param name="filter">The filter containing the search criteria.</param>
        /// <returns>A list of <see cref="Patient"/>s. There is an upper limit on the number of patients that can be returned.</returns>
        public List<Patient> GetUpdatedPatients(UpdatedPatientsSearchFilter filter)
        {
            try
            {
                List<Patient> patients = WebApi.GetUpdatedPatients(ResourceServerBaseAddress, filter,Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);

                return patients;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::GetUpdatedPatients: {0}", e);
                throw;
            }
        }


        #endregion

        #region Session

        /// <summary>
        /// Gets the sessions.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <returns>A list of <see cref="Session"/>s.</returns>
        public List<Session> GetSessions(int patientId)
        {
            try
            {
                return WebApi.GetSessions(ResourceServerBaseAddress, patientId, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::GetSessions: {0}", e);
                throw;
            }
        }

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>The <see cref="Session"/></returns>
        public Session GetSession(int patientId, int sessionId)
        {
            try
            {
                return WebApi.GetSession(ResourceServerBaseAddress, patientId, sessionId, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::GetSession: {0}", e);
                throw;
            }
        }

        #endregion

        #region Action

        /// <summary>
        /// Gets the actions for the specified session of a particular patient.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>A list of <see cref="Action"/>s.</returns>
        public List<Action> GetActions(int patientId, int sessionId)
        {
            try
            {
                return WebApi.GetActions(ResourceServerBaseAddress, patientId, sessionId, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::GetActions (from session): {0}", e);
                throw;
            }
        }

        /// <summary>
        /// Gets all actions for a patient.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <returns>A list of <see cref="Action"/>s.</returns>
        public List<Action> GetActions(int patientId)
        {
            try
            {
                return WebApi.GetActions(ResourceServerBaseAddress, patientId, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::GetActions: {0}", e);
                throw;
            }
        }

        /// <summary>
        /// Gets all actions for a patient.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <param name="actionFilter">Filtering requests.</param>
        /// <returns>A list of <see cref="Action"/>s.</returns>
        public List<Action> GetActions(int patientId, ActionFilter actionFilter)
        {
            try
            {
                return WebApi.GetActions(ResourceServerBaseAddress, patientId, actionFilter, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::GetActions (filtered): {0}", e);
                throw;
            }
        }

        /// <summary>
        /// Creates the action.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <param name="action">The action.</param>
        /// <returns>A copy of the created <see cref="Action"/>.</returns>
        public Action CreateAction(int patientId, Action action)
        {
            try
            {
                action.ModuleID = ModuleId;
                return WebApi.CreateAction(ResourceServerBaseAddress, patientId, action, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::CreateAction: {0}", e);
                throw;
            }
        }

        /// <summary>
        /// Updates the action.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <param name="action">The action.</param>
        /// <returns>A copy of the updated <see cref="Action"/>.</returns>
        public Action UpdateAction(int patientId, Action action)
        {
            try
            {
                action.ModuleID = ModuleId;
                return WebApi.UpdateAction(ResourceServerBaseAddress, patientId, action.SessionID, action, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::UpdateAction: {0}", e);
                throw;
            }
        }

        /// <summary>
        /// Gets the action.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="actionId">The action identifier.</param>
        /// <returns>The <see cref="Action"/>.</returns>
        public Action GetAction(int patientId, int sessionId, int actionId)
        {
            try
            {
                return WebApi.GetAction(ResourceServerBaseAddress, patientId, sessionId, actionId, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::GetAction: {0}", e);
                throw;
            }
        }

        /// <summary>
        /// Gets the action references.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="actionId">The action identifier.</param>
        /// <returns>An array of the ids of the referenced actions.</returns>
        public int[] GetActionReferences(int patientId, int sessionId, int actionId)
        {
            try
            {
                return WebApi.GetActionReferences(ResourceServerBaseAddress, patientId, sessionId, actionId, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::GetActionReferences: {0}", e);
                throw;
            }
        }

        /// <summary>
        /// Updates the action references.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="actionId">The action identifier.</param>
        /// <param name="references">The references.</param>
        public void UpdateActionReferences(int patientId, int sessionId, int actionId, int[] references)
        {
            try
            {
                WebApi.UpdateActionReferences(ResourceServerBaseAddress, patientId, sessionId, actionId, references, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::UpdateActionReferences: {0}", e);
                throw;
            }
        }

        /// <summary>
        /// Gets the action public data.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="actionId">The action identifier.</param>
        /// <returns>A ´blob´ of binary data.</returns>
        public byte[] GetActionPublicData(int patientId, int sessionId, int actionId)
        {
            try
            {
                return WebApi.GetActionPublicData(ResourceServerBaseAddress, patientId, sessionId, actionId, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::GetActionPublicData: {0}", e);
                throw;
            }
        }

        /// <summary>
        /// Updates the actions public data.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="actionId">The action identifier.</param>
        /// <param name="publicData">The public data.</param>
        public void UpdateActionPublicData(int patientId, int sessionId, int actionId, byte[] publicData)
        {
            try
            {
                WebApi.UpdateActionPublicData(ResourceServerBaseAddress, patientId, sessionId, actionId, publicData, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::UpdateActionPublicData: {0}", e);
                throw;
            }
        }

        /// <summary>
        /// Gets the action private data.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="actionId">The action identifier.</param>
        /// <returns>A ´blob´ of binary data.</returns>
        public byte[] GetActionPrivateData(int patientId, int sessionId, int actionId)
        {
            try
            {
                return WebApi.GetActionPrivateData(ResourceServerBaseAddress, patientId, sessionId, actionId, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::GetActionPrivateData: {0}", e);
                throw;
            }
        }

        /// <summary>
        /// Updates the actions private data.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="actionId">The action identifier.</param>
        /// <param name="privateData">The private data.</param>
        public void UpdateActionPrivateData(int patientId, int sessionId, int actionId, byte[] privateData)
        {
            try
            {
                WebApi.UpdateActionPrivateData(ResourceServerBaseAddress, patientId, sessionId, actionId, privateData, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::UpdateActionPrivateData: {0}", e);
                throw;
            }
        }

        /// <summary>
        /// Gets a list of updated actions between a timeinterval and a list of wanted data types.
        /// </summary>
        /// <param name="filter">The filter for the search.</param>
        /// <returns>A list of <see cref="ActionEx"/>s. There is an upper limit on the number of actions that can be returned.</returns>
        public List<ActionEx> GetUpdatedActions(UpdatedActionsSearchFilter filter)
        {
            try
            {
                List<ActionEx> actions = WebApi.GetUpdatedActions(ResourceServerBaseAddress, filter, Token, RemoteHostId, ClientId, DeviceId, ModuleId, Timeout);
                return actions;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.AccessLayer::GetUpdatedActions: {0}", e);
                throw;
            }
        }

        #endregion
    }
}
