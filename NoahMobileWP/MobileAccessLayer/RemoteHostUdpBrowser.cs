using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sockets.Plugin;

namespace Himsa.Noah.MobileAccessLayer
{
    public class RemoteHostDiscoveredEventArgs : EventArgs
    {
        public List<ServerData> Server { get; private set; }

        public RemoteHostDiscoveredEventArgs(List<ServerData> server)
        {
            Server = server;
        }
    }
    public class RemoteHostUdpBrowser
    {
        public static event EventHandler<RemoteHostDiscoveredEventArgs> RemoteHostDiscovered;
        private const int TxPort = 8300;
        private const int RxPort = 8400;
        private static readonly byte[] DiscoveryMessage = Encoding.UTF8.GetBytes("{B0BE0E7D-F70B-40BE-91AB-14125863B0B7}");
        private const string ServerId = "{5FE140D5-1D3F-4E46-8892-15FA89DAE9F4}";
        private static UdpSocketClient transmitSocket;
        private static UdpSocketReceiver receiveSocket;
        //private static List<ServerData> ServerList = null;
        private static List<ServerData> serverList = new List<ServerData>();
        ~RemoteHostUdpBrowser()
        {
            StopDiscovery();
        }

        private static async Task<string> StartDiscovery(Action<List<ServerData>> onServerDiscovered, string ipAddr)
        {
            //if (transmitSocket == null && receiveSocket == null)
            //{
            
            transmitSocket = new UdpSocketClient();
            string addr = string.Empty;
            receiveSocket.MessageReceived += (sender, args) =>
            {
                var fromIp = args.RemoteAddress;
                var data = args.ByteData;
                LocalRemoteHostInfo payload;
                try
                {
                    payload = Helpers.Deserialize<LocalRemoteHostInfo>(Encoding.UTF8.GetString(data, 0, data.Length));
                }
                catch (Exception e)
                {
                    Debug.WriteLine(
                        string.Format("Caught {0} while attempting to deserialize local discovery response", e));
                    return;
                }
                var id = payload.LocalNoahServerGuid;
                if (!string.Equals(id, ServerId, StringComparison.CurrentCultureIgnoreCase)) return;
                if (serverList.Count > 0)
                   {
                        var server = serverList.Find(element => element.FriendlyName == payload.FriendlyName);
                        if (server == null)
                        {
                            serverList.Add(new ServerData
                            {
                                FriendlyName = payload.FriendlyName,
                                RemoteHostId = payload.RemoteHostId,
                                ServerName = payload.HostName,
                                ServerIp = payload.LocalHostIpAddress
                            });
                        }
                    }
                    else
                    {
                        serverList.Add(new ServerData
                        {
                            FriendlyName = payload.FriendlyName,
                            RemoteHostId = payload.RemoteHostId,
                            ServerName = payload.HostName,
                            ServerIp = payload.LocalHostIpAddress
                        });
                    }
                if (addr.Equals("255.255.255.255"))
                   {
                       var server = serverList.First(element => element.FriendlyName == payload.FriendlyName);
                       if (server == null)
                       {
                           serverList.Add(new ServerData
                           {
                               FriendlyName = payload.FriendlyName,
                               RemoteHostId = payload.RemoteHostId,
                               ServerName = payload.HostName,
                               ServerIp = payload.LocalHostIpAddress
                           });
                       }
                       onServerDiscovered(serverList);
                   }
            };
            for (int i = 3; i >= 0;)
            {
                addr = GetBroadcastAddress(ipAddr, i);
                await transmitSocket.SendToAsync(DiscoveryMessage, addr, TxPort);
                  i = i - 1;
            }
            return ipAddr;
            //}
        }

        private static void StopDiscovery()
        {
            if (null != transmitSocket)
                transmitSocket.DisconnectAsync();
            if (null != receiveSocket)
                receiveSocket.StopListeningAsync();
        }

        public static async void GetListOfLocalHosts(string ipAddr)
        {
            
            receiveSocket = new UdpSocketReceiver();
            await receiveSocket.StartListeningAsync(RxPort);
            
               
               
              await StartDiscovery(OnServerDiscovered, ipAddr);
               // Wait until the control thread is done before proceeding. 
               // This keeps the inactivity timers from overlapping.
             
            
        }

        private static void OnServerDiscovered(List<ServerData> serverData)
        {
            var handler = RemoteHostDiscovered;
            if (null == handler) return;
            handler(null, new RemoteHostDiscoveredEventArgs(serverData));

        }

        private static string GetBroadcastAddress(String localAddress, int take)
        {
            var parts = localAddress.Split(new[] { '.' }).Take(take);

            if (take == 3)
                return string.Join(".", parts) + ".255";
            if (take == 2)
                return string.Join(".", parts) + ".255.255";
            if (take == 1)
                return string.Join(".", parts) + ".255.255.255";
            if (take == 0)
                return string.Join(".", parts) + "255.255.255.255";

            return string.Join(".", parts) + "255.255.255.255";
        }
    }

}
