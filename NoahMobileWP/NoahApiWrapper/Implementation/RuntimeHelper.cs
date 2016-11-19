using Himsa.Noah.MobileAccessLayer;
using System;
using System.Linq;
namespace NoahApiWrapper
{
    internal class ConnectionHelper
    {
        public static Uri ResourceServer = new Uri("https://apiexttest.noahmobile.net/");
        public static Uri AuthServer = new Uri("https://authexttest.noahmobile.net");
        public static string DemoAppClientId = "urn.clientId.dfd6eb84-28ca-48de-a2ff-e344a733b4a6";

        public static ServerInfo GetServer(string friendlyName)
        {
            var connection = ConnectionPreferenceType.CloudOnly;
            var response = AccessLayer.Discovery(
                ResourceServer,
                friendlyName,
                ref connection,
                "",
                TimeSpan.FromSeconds(10d));

            return new ServerInfo
            {
                FriendlyName = friendlyName,
                RemoteHostId = response.RemoteHostId,
                LocalIp = response.LocalHostIpAddress,
            };
        }

        public static ServerInfo[] DiscoverLocalServers()
        {
            return Enumerable.Empty<ServerInfo>().ToArray();
        }


        internal static Uri GetAuthenticationUri(Configuration configuration, ServerInfo server)
        {
            var uri = AccessLayer.GetImplicitGrantAuthenticationUri(
                AuthServer, 
                "/oauth/authorize", 
                DemoAppClientId,
                server.RemoteHostId.ToString(), 
                "", 
                configuration.Alias);

            return uri;
        }
    }

    public class ServerInfo
    {
        public bool Equals(ServerInfo other)
        {
            return string.Equals(FriendlyName, other.FriendlyName) && RemoteHostId.Equals(other.RemoteHostId) && Equals(LocalIp, other.LocalIp);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ServerInfo && Equals((ServerInfo)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (FriendlyName != null ? FriendlyName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ RemoteHostId.GetHashCode();
                hashCode = (hashCode * 397) ^ (LocalIp != null ? LocalIp.GetHashCode() : 0);
                return hashCode;
            }
        }

        public string FriendlyName { get; set; }
        public Guid RemoteHostId { get; set; }
        public string LocalIp { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}]({1} / {2})", FriendlyName, RemoteHostId, LocalIp);
        }

        public static bool operator ==(ServerInfo a, ServerInfo b)
        {
            return (ReferenceEquals(null, a) && ReferenceEquals(null, b)) ||
                   (!ReferenceEquals(null, a) && !ReferenceEquals(null, b)) &&
                   (a.FriendlyName == b.FriendlyName &&
                    Equals(a.LocalIp, b.LocalIp) &&
                    a.RemoteHostId == b.RemoteHostId);
        }

        public static bool operator !=(ServerInfo a, ServerInfo b)
        {
            return !(a == b);
        }
    }
}
