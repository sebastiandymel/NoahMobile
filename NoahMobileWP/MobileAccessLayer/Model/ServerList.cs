using System;

namespace Himsa.Noah.MobileAccessLayer
{
    public class ServerList
    {
        public ServerData[] Servers { get; set; }
    }
    public class ServerData
    {
        public string ServerName { get; set; }
        public string ServerVersion { get; set; }
        public string ServerIp { get; set; }
        public string FriendlyName { get; set; }
        public Guid RemoteHostId { get; set; }
    }
}
