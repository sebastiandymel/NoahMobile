using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;

namespace Himsa.Noah.MobileAccessLayer
{
    internal class NoahServerUdpBrowser
    {

        public static List<ServerData> ServerCollection = new List<ServerData>();
        const int port = 8300;
        const int listenPort = 8400;
        public static bool messageReceived = false;
        static bool waitTime = true;

        async public static Task<ServerList> BrowseForNoahServers(string friendlyName, string ipAddress)
        {
            var client = new UdpSocketClient();
            var receiver = new UdpSocketReceiver();

            try
            {
                var address = GetIpAddressForBroadcast(ipAddress);            
                //// convert our greeting message into a byte array
                string clientGUID = "{B0BE0E7D-F70B-40BE-91AB-14125863B0B7}";
                System.Text.Encoding enc = System.Text.Encoding.UTF8;
                byte[] sendBuffer = enc.GetBytes(clientGUID);

                receiver.MessageReceived += (sender, args) =>
                {
                    try
                    {
                        var from = String.Format("{0}:{1}", args.RemoteAddress, args.RemotePort);
                        var fromIp = args.RemoteAddress;
                        var data = Encoding.UTF8.GetString(args.ByteData, 0, args.ByteData.Length);
                        
                        try
                        {
                            LocalRemoteHostInfo payload =
                            Helpers.Deserialize<LocalRemoteHostInfo>(Encoding.UTF8.GetString(args.ByteData, 0,
                             args.ByteData.Length));
                            string guid = payload.LocalNoahServerGuid;
                            if (guid.Equals("{5FE140D5-1D3F-4E46-8892-15FA89DAE9F4}"))
                            {
                                bool duplicate = false;
                                foreach (ServerData servData in ServerCollection)
                                {
                                    if (servData.ServerIp.Equals(fromIp))
                                    {
                                        duplicate = true; //The adress is allready in the list
                                        break;
                                    }
                                }
                                if (duplicate == false) //No need to list Gatteway IP
                                {
                                    if (string.IsNullOrEmpty(friendlyName))
                                    {
                                        ServerCollection.Add(new ServerData
                                        {
                                            ServerName = payload.HostName,
                                            FriendlyName = payload.FriendlyName,
                                            ServerIp = payload.LocalHostIpAddress,
                                            RemoteHostId = payload.RemoteHostId
                                        });
                                    }
                                    else
                                    {
                                        if (friendlyName == payload.FriendlyName)
                                        {
                                            receiver.StopListeningAsync();
                                            ServerCollection.Add(new ServerData
                                            {
                                                ServerName = payload.HostName,
                                                FriendlyName = payload.FriendlyName,
                                                ServerIp = payload.LocalHostIpAddress,
                                                RemoteHostId = payload.RemoteHostId
                                            });
                                            
                                            //client.Dispose();
                                            //receiver.Dispose();
                                            waitTime = false;
                                        }
                                    }
                                } 
                            }
                        }
                        catch { }
                    }
                    catch { }
                    
                };
                try
                {
                    receiver.StartListeningAsync(listenPort);
                }
                catch (Exception e)
                {
                }
                client.ConnectAsync(address, port);
                client.SendAsync(sendBuffer);
               
                    DateTime now = DateTime.Now;
                    DateTime stop = now + new TimeSpan(0, 0, 0, 5);


                    while (waitTime)
                    {

                        if (DateTime.Now > stop)
                            waitTime = false;
                    }
                
              
        
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.NoahServerUdpBrowser::BrowseForNoahServers(): {0}", e);
                throw;

            }
            finally
            {
                client.DisconnectAsync();
                receiver.StopListeningAsync();
                receiver.Dispose();
            }

            return new ServerList
            {
                Servers = ServerCollection.ToArray()
            };
              

            }

        private static string GetIpAddressForBroadcast(string ipAddress)
        {
            string ipResult = ipAddress;
            char[] delimiters = new char[] {'.'};
            string[] numbers = ipAddress.Split(delimiters,4);
            if (numbers.Length >= 3)
            {
                numbers[3] = "255";
                ipResult = numbers[0] + "." + numbers[1] + "." + numbers[2] + "." + numbers[3];
            }

            return ipResult;
            
        }

            
        }

    }

