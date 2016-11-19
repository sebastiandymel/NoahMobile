using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using Newtonsoft.Json;

namespace Himsa.Noah.MobileAccessLayer
{
    /// <summary>
    /// Various helper methods.
    /// </summary>
    internal class Helpers
    {
        #region HTTP

        public enum HttpVerb
        {
            Get,
            Put,
            Post,
            Delete
        }

        public static Uri AppendQuery(Uri uri, string name, string value)
        {
            try
            {
                var uriBuilder = new UriBuilder(uri)
                {
                    Query = (string.IsNullOrEmpty(uri.Query) ? "" : uri.Query.Remove(0, 1) + "&") + name + "=" + value
                };
                return uriBuilder.Uri;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.Helpers::AppendQuery: {0}", e);
                throw;
            }
        }

        public static T Get<T>(Uri baseAddress, string path, Token token, string remoteHostId, string clientId, string deviceId, int? moduleId, TimeSpan timeout)
        {
            try
            {
                return Execute<object, T>(HttpVerb.Get, baseAddress, path, token, remoteHostId, clientId, deviceId, moduleId, null, timeout);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.Helpers::Get<T>: {0}", e);
                throw;
            }
        }

        public static TResult Execute<TData, TResult>(HttpVerb verb, Uri baseAddress, string path, Token token,
            string remoteHostId, string clientId, string deviceId, int? moduleId, TData data, TimeSpan timeout)
        {
            try
            {
                Log.DebugFormat(
                    "Himsa.Noah.MobileAccessLayer.Helpers::Execute<TData, TResult>: verb={0} baseAddress={1} path={2} accessToken={3} remoteHostId={4} clientId={5} deviceId={6} moduleId={7} timeout={8}",
                    verb, baseAddress, path,
                    token == null || token.AccessToken == null
                        ? ""
                        : ("..." + token.AccessToken.Substring(Math.Max(0, token.AccessToken.Length - 6))), remoteHostId,
                    clientId, deviceId, moduleId, timeout);
                HttpClientHandler messageHandler = new HttpClientHandler();
                messageHandler.ClientCertificateOptions = ClientCertificateOption.Automatic;
                messageHandler.AllowAutoRedirect = true;
                messageHandler.UseDefaultCredentials = true;
                messageHandler.PreAuthenticate = true;
                
                //messageHandler.Credentials = 
                using (var client = new HttpClient(messageHandler))
                {
                   
                    string responsestring = "";
                    try
                    {
                        client.Timeout = timeout;
                        client.BaseAddress = baseAddress;
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        // Add the RemoteHostId header
                        if (!string.IsNullOrEmpty(remoteHostId))
                            client.DefaultRequestHeaders.Add("X-RemoteHostId", remoteHostId);

                        // Add the DeviceId and ApplicationId and ModuleId header
                        if (!string.IsNullOrEmpty(deviceId))
                            client.DefaultRequestHeaders.Add("X-DeviceId", deviceId);
                        if (!string.IsNullOrEmpty(clientId))
                            client.DefaultRequestHeaders.Add("X-ApplicationId", clientId);
                        if (moduleId.HasValue)
                            client.DefaultRequestHeaders.Add("X-ModuleId",
                                moduleId.Value.ToString(CultureInfo.InvariantCulture));

                        // Add access token
                        if (token != null && !string.IsNullOrEmpty(token.AccessToken))
                            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);

                        HttpResponseMessage response;
                        var serializedData = Serialize(data);
                        HttpContent contentPost = new StringContent(serializedData, Encoding.UTF8, "application/json");
                        switch (verb)
                        {
                            case HttpVerb.Put:
                                response = client.PutAsync(path, contentPost).Result;
                                break;
                            case HttpVerb.Post:
                                response = client.PostAsync(path, contentPost).Result;
                                break;
                            case HttpVerb.Delete:
                                response = client.DeleteAsync(path).Result;
                                break;
                            case HttpVerb.Get:
                                response = client.GetAsync(path).Result;
                                break;
                            default:
                                throw new AccessLayerException(
                                    string.Format("Unsupported verb: Verb='{0}', baseAddress='{1}', path='{2}'", verb,
                                        baseAddress, path));
                        }

                        responsestring = response.Content.ReadAsStringAsync().Result ?? "";
                        if (HttpStatusCode.InternalServerError == response.StatusCode)
                        {
                            NoahMobileFault fault;
                            try
                            {
                                fault = Deserialize<NoahMobileFault>(responsestring);
                            }
                            catch
                            {
                                fault = null;
                            }

                            if (fault != null)
                            {
                                throw new AccessLayerNoahMobileException(fault);
                            }
                        }
                        response.EnsureSuccessStatusCode();
                        return Deserialize<TResult>(responsestring);
                    }
                    catch (AccessLayerNoahMobileException)
                    {
                        throw;
                    }
                    catch (Exception e)
                    {
                        var newException = new AccessLayerExceptionWithObject(e.Message + " See inner exception.",
                            responsestring, e);
                        throw newException;
                    }
                }
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.Helpers::Execute<TData, TResult>: {0}", e);
                throw;
            }
        }

        public static Dictionary<string, string> ParseQueryString(Uri uri)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.Helpers::ParseQueryString: uri={0}", uri);
                var query = uri.Query.Substring(uri.Query.IndexOf('?') + 1); // +1 for skipping '?'
                var pairs = query.Split('&');
                return pairs
                    .Select(o => o.Split('='))
                    .Where(items => items.Count() == 2)
                    .ToDictionary(pair => Uri.UnescapeDataString(pair[0]),
                        pair => Uri.UnescapeDataString(pair[1]));
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.Helpers::ParseQueryString: {0}", e);
                throw;
            }
        }

        #endregion

        #region Serialization

        public static T Deserialize<T>(string str)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(str);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.Helpers::Deserialize<T>: {0}", e);
                throw;
            }
        }

        public static string Serialize<T>(T obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.Helpers::Serialize<T>: {0}", e);
                throw;
            }
        }

        #endregion

        #region Validation

        public static bool IsValid(Patient patient)
        {
            try
            {
                return patient != null && patient.Id > 0;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.Helpers::IsValid(Patient): {0}", e);
                throw;
            }
        }

        public static bool IsValid(Action action)
        {
            try
            {
                return action != null && action.Id > 0;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.Helpers::IsValid(Action): {0}", e);
                throw;
            }
        }

        #endregion

    }
}
