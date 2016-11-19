using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Himsa.Noah.MobileAccessLayer
{
    /// <summary>
    /// Wraps the Noah Mobile Web API.
    /// </summary>
    internal class WebApi
    {
        internal static List<String> LocalServerListThread(string ipAddress)
        {
            ServerList servers = new ServerList();
            servers = NoahServerUdpBrowser.BrowseForNoahServers(string.Empty, ipAddress).Result;
            List<String> resultList = new List<string>();
            foreach (var VARIABLE in servers.Servers)
            {
                resultList.Add(VARIABLE.FriendlyName);
            }

            return resultList;
        }
        
        public static DiscoveryResponse Discovery(Uri resourceServerBaseAddress, string friendlyName,
            ref ConnectionPreferenceType connectionpreference, string ipAddress, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::Discovery called.");
                string path = String.Format("api/discovery/{0}/preference/{1}", friendlyName, connectionpreference);
                var discoveryReponse = new DiscoveryResponse {RemoteHostId = Guid.Empty};
                if (connectionpreference == ConnectionPreferenceType.CloudOnly)
                {
                    discoveryReponse = Helpers.Get<DiscoveryResponse>(resourceServerBaseAddress, path, null, null,
                        null,
                        null, null, timeout);
                }
                else
                {
                    ServerList data = NoahServerUdpBrowser.BrowseForNoahServers(friendlyName, ipAddress).Result;
                    foreach (var server in data.Servers)
                    {
                        discoveryReponse = new DiscoveryResponse
                            {
                                LocalHostIpAddress = server.ServerIp,
                                RemoteHostId = server.RemoteHostId
                            };
                        
                    }
                    
                    if ((connectionpreference == ConnectionPreferenceType.LocalPrefered) && (discoveryReponse.RemoteHostId == Guid.Empty))
                    {
                        discoveryReponse = Helpers.Get<DiscoveryResponse>(resourceServerBaseAddress, path, null, null,
                        null,
                        null, null, timeout);
                        connectionpreference = ConnectionPreferenceType.CloudOnly;
                    }
                }
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::Discovery: {0} found.", discoveryReponse == null ? 0 : 1);
                return discoveryReponse;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::Discovery: {0}", e);
                throw;
            }
        }

        public static AppRegInfo RegisterApp(Uri resourceServerBaseAddress, AppRegInfo registrationData, Token token,
            string remoteHostId, string clientId, string deviceId, int moduleId, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::RegisterApp called.");
                string path = String.Format("api/apps");
                var appRegInfo = Helpers.Execute<AppRegInfo, AppRegInfo>(Helpers.HttpVerb.Post,
                    resourceServerBaseAddress, path, token, remoteHostId, clientId, deviceId, moduleId, registrationData,
                    timeout);
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::RegisterApp: {0} found.", appRegInfo == null ? 0 : 1);
                return appRegInfo;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::RegisterApp: {0}", e);
                throw;
            }
        }

        public static int UnregisterApp(Uri resourceServerBaseAddress, Token token, string remoteHostId, string clientId,
            string deviceId, int moduleId, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::UnregisterApp called.");
                string path = String.Format("api/apps/{0}", moduleId);
                int result = Helpers.Execute<object, int>(Helpers.HttpVerb.Delete, resourceServerBaseAddress, path, token,
                    remoteHostId, clientId, deviceId, moduleId, null, timeout);
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::UnregisterApp: result={0}", result);
                return result;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::UnregisterApp: {0}", e);
                throw;
            }
        }

        public static List<AppPermission> GetAppPermissions(Uri resourceServerBaseAddress, Token token,
            string remoteHostId, string clientId, string deviceId, int moduleId, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetAppPermissions called.");
                string path = String.Format("/api/apppermissions/{0}", moduleId);
                var appPermissions = Helpers.Get<List<AppPermission>>(resourceServerBaseAddress, path, token,
                    remoteHostId, clientId, deviceId, moduleId, timeout);
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetAppPermissions: {0} found.", appPermissions == null ? 0 : appPermissions.Count);
                return appPermissions;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetAppPermissions: {0}", e);
                throw;
            }
        }

        public static void AddAuditTrailEntry(Uri resourceServerBaseAddress, string logEntry, Token token, string remoteHostId, string clientId, string deviceId, int moduleId, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::HippaLogEntry called.");
                string path = String.Format("api/audittrails");
                Helpers.Execute<string,object>(Helpers.HttpVerb.Post, resourceServerBaseAddress,path,token,remoteHostId,clientId,deviceId,moduleId,logEntry,timeout);

                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::HippaLogEntry: added.");
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::HippaLogEntry: {0}", e);
                throw;
            }
        }

        public static NoahServerSettings GetNoahServerSettings(Uri resourceServerBaseAddress, Token token,
          string remoteHostId, string clientId, string deviceId, int moduleId, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetNoahServerSettings called.");
                string path = String.Format("/api/noahserversettings");
                var noahServerSettings = Helpers.Get<NoahServerSettings>(resourceServerBaseAddress, path, token,
                    remoteHostId, clientId, deviceId, moduleId, timeout);
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetNoahServerSettings: found.");
                return noahServerSettings;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetAppPermissions: {0}", e);
                throw;
            }
        }

        public static Patient CreatePatient(Uri resourceServerBaseAddress, Patient patient, Token token,
            string remoteHostId, string clientId, string deviceId, int moduleId, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::CreatePatient called.");
                string path = "api/patients";
                var newPatient = Helpers.Execute<Patient, Patient>(Helpers.HttpVerb.Post, resourceServerBaseAddress,
                    path, token, remoteHostId, clientId, deviceId, moduleId, patient, timeout);
                if (!Helpers.IsValid(newPatient))
                {
                    if (newPatient == null)
                        throw new AccessLayerException(string.Format("Returned value for created patient is null."));
                    else
                        throw new AccessLayerExceptionWithObject(
                            string.Format("Returned value for created patient is invalid. ID='{0}'.", newPatient.Id),
                            patient);
                }
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::CreatePatient: {0} created.", newPatient == null ? 0 : 1);
                return newPatient;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::CreatePatient: {0}", e);
                throw;
            }
        }

        public static Patient UpdatePatient(Uri resourceServerBaseAddress, Patient patient, Token token,
            string remoteHostId, string clientId, string deviceId, int moduleId, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::UpdatePatient called.");
                string path = "api/patients";
                var updatedPatient = Helpers.Execute<Patient, Patient>(Helpers.HttpVerb.Put, resourceServerBaseAddress,
                    path, token, remoteHostId, clientId, deviceId, moduleId, patient, timeout);
                if (!Helpers.IsValid(updatedPatient))
                {
                    if (updatedPatient == null)
                        throw new AccessLayerException(string.Format("Returned value for updated patient is null."));
                    else
                        throw new AccessLayerExceptionWithObject(
                            string.Format("Returned value for updated patient is invalid. ID='{0}'.", updatedPatient.Id),
                            patient);
                }
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::UpdatePatient: Updated.");
                return updatedPatient;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::UpdatePatient: {0}", e);
                throw;
            }
        }

        public static Patient GetPatient(Uri resourceServerBaseAddress, int patientId, Token token, string remoteHostId,
            string clientId, string deviceId, int moduleId, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetPatient called.");
                string path = String.Format("api/patients/{0}", patientId);
                var patient = Helpers.Get<Patient>(resourceServerBaseAddress, path, token, remoteHostId, clientId,
                    deviceId, moduleId, timeout);
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetPatient: {0} found.", patient == null ? 0 : 1);
                return patient;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetPatient: {0}", e);
                throw;
            }
        }

        public static List<Patient> GetPatients(Uri resourceServerBaseAddress, string searchText, Token token,
            string remoteHostId, string clientId, string deviceId, int moduleId, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetPatients called.");
                string path = String.Format("api/patients?searchText={0}", searchText);
                var patients = Helpers.Get<IEnumerable<Patient>>(resourceServerBaseAddress, path, token, remoteHostId,
                    clientId, deviceId, moduleId, timeout);
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetPatients: {0} found.", patients == null ? 0 : patients.Count());
                return patients != null ? patients.ToList() : null;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetPatients: {0}", e);
                throw;
            }
        }

        public static PatientSearch GetPatients(Uri resourceServerBaseAddress, string searchText, int? page,
            int? pageSize, Token token, string remoteHostId, string clientId, string deviceId, int moduleId,
            TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetPatients called.");
                string path = String.Format("api/patients?searchText={0}", searchText);
                if (page.HasValue)
                    path += String.Format("&page={0}", page.Value);
                if (pageSize.HasValue)
                    path += String.Format("&pagesize={0}", pageSize.Value);
                var patientSearch = Helpers.Get<PatientSearch>(resourceServerBaseAddress, path, token, remoteHostId,
                    clientId, deviceId, moduleId, timeout);
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetPatients: {0} found.", patientSearch == null || patientSearch.Patients == null ? 0 : patientSearch.Patients.Count());
                return patientSearch ?? null;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetPatients: {0}", e);
                throw;
            }
        }
        
        public static List<Patient> GetUpdatedPatients(Uri resourceServerBaseAddress, UpdatedPatientsSearchFilter filter, Token token, string remoteHostId, string clientId, string deviceId, int moduleId,
           TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetUpdatedPatients called.");
                string path = String.Format("api/patients/updates");

                var patients = Helpers.Execute<UpdatedPatientsSearchFilter, PatientSearch>(Helpers.HttpVerb.Post, resourceServerBaseAddress, path, token, remoteHostId, clientId, deviceId, moduleId, filter, timeout);
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetUpdatedPatients: {0} found.", patients == null ? 0 : patients.Patients.Count);

                if(patients == null)
                    return null;
                
                return patients.Patients;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetUpdatedPatients: {0}", e);
                throw;
            }
        }
          
        public static List<Session> GetSessions(Uri resourceServerBaseAddress, int patientId, Token token,
            string remoteHostId, string clientId, string deviceId, int moduleId, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetSessions called.");
                string path = String.Format("api/patients/{0}/sessions", patientId);
                var sessions = Helpers.Get<IEnumerable<Session>>(resourceServerBaseAddress, path, token, remoteHostId,
                    clientId, deviceId, moduleId, timeout);
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetSessions: {0} found.", sessions == null ? 0 : sessions.Count());
                return sessions != null ? sessions.ToList() : null;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetSessions: {0}", e);
                throw;
            }
        }

        public static Session GetSession(Uri resourceServerBaseAddress, int patientId, int sessionId, Token token,
            string remoteHostId, string clientId, string deviceId, int moduleId, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetSession called.");
                string path = String.Format("api/patients/{0}/sessions/{1}", patientId, sessionId);
                var session = Helpers.Get<Session>(resourceServerBaseAddress, path, token, remoteHostId, clientId,
                    deviceId, moduleId, timeout);
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetSession: {0} found.", session == null ? 0 : 1);
                return session;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetSession: {0}", e);
                throw;
            }
        }

        public static Action CreateAction(Uri resourceServerBaseAddress, int patientId, Action action, Token token,
            string remoteHostId, string clientId, string deviceId, int moduleId, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::CreateAction called.");
                string path = String.Format("api/patients/{0}/actions", patientId);
                var newAction = Helpers.Execute<Action, Action>(Helpers.HttpVerb.Post, resourceServerBaseAddress, path,
                    token, remoteHostId, clientId, deviceId, moduleId, action, timeout);
                if (!Helpers.IsValid(newAction))
                {
                    if (newAction == null)
                        throw new AccessLayerException(string.Format("Returned value for created action is null."));
                    else
                        throw new AccessLayerExceptionWithObject(
                            string.Format("Returned value for created action is invalid. ID='{0}'.", newAction.Id),
                            action);
                }
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::CreateAction: {0} created.", newAction == null ? 0 : 1);
                return newAction;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::CreateAction: {0}", e);
                throw;
            }
        }

        public static Action UpdateAction(Uri resourceServerBaseAddress, int patientId, int sessionId, Action action,
            Token token, string remoteHostId, string clientId, string deviceId, int moduleId, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::UpdateAction called.");
                string path = String.Format("api/patients/{0}/sessions/{1}/actions/{2}", patientId, sessionId, action.Id);
                var updatedAction = Helpers.Execute<Action, Action>(Helpers.HttpVerb.Put, resourceServerBaseAddress,
                    path, token, remoteHostId, clientId, deviceId, moduleId, action, timeout);
                if (!Helpers.IsValid(updatedAction))
                {
                    if (updatedAction == null)
                        throw new AccessLayerException(string.Format("Returned value for updated action is null."));
                    else
                        throw new AccessLayerExceptionWithObject(
                            string.Format("Returned value for updated action is invalid. ID='{0}'.", updatedAction.Id),
                            action);
                }
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::UpdateAction: {0} updated.", updatedAction == null ? 0 : 1);
                return updatedAction;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::UpdateAction: {0}", e);
                throw;
            }
        }

        public static List<Action> GetActions(Uri resourceServerBaseAddress, int patientId, int sessionId, Token token,
            string remoteHostId, string clientId, string deviceId, int moduleId, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetActions (from sessions) called.");
                string path = String.Format("api/patients/{0}/sessions/{1}/actions", patientId, sessionId);
                var actions = Helpers.Get<IEnumerable<Action>>(resourceServerBaseAddress, path, token, remoteHostId,
                    clientId, deviceId, moduleId, timeout);
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetActions (from sessions): {0} found.", actions == null ? 0 : actions.Count());
                return actions != null ? actions.ToList() : null;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetActions (from sessions): {0}", e);
                throw;
            }
        }

        public static List<Action> GetActions(Uri resourceServerBaseAddress, int patientId, Token token,
            string remoteHostId, string clientId, string deviceId, int moduleId, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetActions called.");
                string path = String.Format("api/patients/{0}/actions", patientId);
                var actions = Helpers.Get<IEnumerable<Action>>(resourceServerBaseAddress, path, token, remoteHostId,
                    clientId, deviceId, moduleId, timeout);
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetActions: {0} found.", actions == null ? 0 : actions.Count());
                return actions != null ? actions.ToList() : null;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetActions: {0}", e);
                throw;
            }
        }

        public static List<Action> GetActions(Uri resourceServerBaseAddress, int patientId, ActionFilter actionfilter,
            Token token, string remoteHostId, string clientId, string deviceId, int moduleId, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetActions (filtered) called.");
                string path = String.Format("api/patients/{0}/actions/filter", patientId);
                var actions = Helpers.Execute<ActionFilter, IEnumerable<Action>>(Helpers.HttpVerb.Post,
                    resourceServerBaseAddress, path, token, remoteHostId, clientId, deviceId, moduleId, actionfilter,
                    timeout);
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetActions (filtered): {0} found.",
                    actions == null ? 0 : actions.Count());
                return actions != null ? actions.ToList() : null;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetActions (filtered): {0}", e);
                throw;
            }
        }

        public static Action GetAction(Uri resourceServerBaseAddress, int patientId, int sessionId, int actionId,
            Token token, string remoteHostId, string clientId, string deviceId, int moduleId, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetAction called.");
                string path = String.Format("api/patients/{0}/sessions/{1}/actions/{2}", patientId, sessionId, actionId);
                var action = Helpers.Get<Action>(resourceServerBaseAddress, path, token, remoteHostId, clientId,
                    deviceId, moduleId, timeout);
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetAction: {0} found.", action == null ? 0 : 1);
                return action;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetAction: {0}", e);
                throw;
            }
        }

        public static int[] GetActionReferences(Uri resourceServerBaseAddress, int patientId, int sessionId,
            int actionId, Token token, string remoteHostId, string clientId, string deviceId, int moduleId,
            TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetActionReferences called.");
                string path = String.Format("api/patients/{0}/sessions/{1}/actions/{2}/references", patientId, sessionId,
                    actionId);
                var ids = Helpers.Get<int[]>(resourceServerBaseAddress, path, token, remoteHostId, clientId, deviceId,
                    moduleId, timeout);
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetActionReferences: {0} found.", ids == null ? 0 : 1);
                return ids;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetActionReferences: {0}", e);
                throw;
            }
        }

        public static void UpdateActionReferences(Uri resourceServerBaseAddress, int patientId, int sessionId,
            int actionId, int[] references, Token token, string remoteHostId, string clientId, string deviceId,
            int moduleId, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::UpdateActionReferences called.");
                string path = String.Format("api/patients/{0}/sessions/{1}/actions/{2}/references", patientId, sessionId,
                    actionId);
                Helpers.Execute<int[], object>(Helpers.HttpVerb.Put, resourceServerBaseAddress, path, token,
                    remoteHostId, clientId, deviceId, moduleId, references, timeout);
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::UpdateActionReferences. Updated.");
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::UpdateActionReferences: {0}", e);
                throw;
            }
        }

        public static byte[] GetActionPublicData(Uri resourceServerBaseAddress, int patientId, int sessionId,
            int actionId, Token token, string remoteHostId, string clientId, string deviceId, int moduleId,
            TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetActionPublicData called.");
                string path = String.Format("api/patients/{0}/sessions/{1}/actions/{2}/publicdata", patientId, sessionId,
                    actionId);
                var dataAsBase64 = Helpers.Get<string>(resourceServerBaseAddress, path, token, remoteHostId, clientId,
                    deviceId, moduleId, timeout);
                var data = Convert.FromBase64String(dataAsBase64);
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetActionPublicData: {0} bytes found.", data == null ? 0 : data.Length);
                return data;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetActionPublicData: {0}", e);
                throw;
            }
        }

        public static void UpdateActionPublicData(Uri resourceServerBaseAddress, int patientId, int sessionId,
            int actionId, byte[] publicData, Token token, string remoteHostId, string clientId, string deviceId,
            int moduleId, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::UpdateActionPublicData called.");
                string path = String.Format("api/patients/{0}/sessions/{1}/actions/{2}/publicdata", patientId, sessionId,
                    actionId);
                var dataAsBase64 = Convert.ToBase64String(publicData);
                Helpers.Execute<string, object>(Helpers.HttpVerb.Put, resourceServerBaseAddress, path, token,
                    remoteHostId, clientId, deviceId, moduleId, dataAsBase64, timeout);
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::UpdateActionPublicData. Updated.");
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::UpdateActionPublicData: {0}", e);
                throw;
            }
        }

        public static byte[] GetActionPrivateData(Uri resourceServerBaseAddress, int patientId, int sessionId,
            int actionId, Token token, string remoteHostId, string clientId, string deviceId, int moduleId,
            TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetActionPrivateData called.");
                string path = String.Format("api/patients/{0}/sessions/{1}/actions/{2}/privatedata", patientId,
                    sessionId, actionId);
                var dataAsBase64 = Helpers.Get<string>(resourceServerBaseAddress, path, token, remoteHostId, clientId,
                    deviceId, moduleId, timeout);
                var data = Convert.FromBase64String(dataAsBase64);
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetActionPrivateData: {0} bytes found.", data == null ? 0 : data.Length);
                return data;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetActionPrivateData: {0}", e);
                throw;
            }
        }

        public static void UpdateActionPrivateData(Uri resourceServerBaseAddress, int patientId, int sessionId,
            int actionId, byte[] privateData, Token token, string remoteHostId, string clientId, string deviceId,
            int moduleId, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::UpdateActionPrivateData called.");
                string path = String.Format("api/patients/{0}/sessions/{1}/actions/{2}/privatedata", patientId,
                    sessionId, actionId);
                var dataAsBase64 = Convert.ToBase64String(privateData);
                Helpers.Execute<string, object>(Helpers.HttpVerb.Put, resourceServerBaseAddress, path, token,
                    remoteHostId, clientId, deviceId, moduleId, dataAsBase64, timeout);
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::UpdateActionPrivateData. Updated.");
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::UpdateActionPrivateData: {0}", e);
                throw;
            }
        }
        
        public static List<ActionEx> GetUpdatedActions(Uri resourceServerBaseAddress, UpdatedActionsSearchFilter filter, Token token, string remoteHostId, string clientId, string deviceId, int moduleId, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetUpdatedActions called.");
                string path = String.Format("api/patients/actions/updates");
                var actions = Helpers.Execute<UpdatedActionsSearchFilter, ActionSearch>(Helpers.HttpVerb.Post, resourceServerBaseAddress, path, token, remoteHostId, clientId, deviceId, moduleId, filter, timeout);
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetUpdatedActions: {0} found.", actions == null ? 0 : actions.ActionExs.Count());
                if (actions == null)
                    return null;
                
                return actions.ActionExs;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::GetUpdatedActions: {0}", e);
                throw;
            }
        }

        public static TimeSpan DefaultTimeout
        {
            get
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        return client.Timeout;
                    }
                }
                catch (Exception e)
                {
                    Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.WebApi::DefaultTimeout: {0}", e);
                    throw;
                }
            }
        }
    }
}
