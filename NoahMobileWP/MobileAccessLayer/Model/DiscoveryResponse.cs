using System;

namespace Himsa.Noah.MobileAccessLayer
{
    /// <summary>
    /// Priority of preferred connection types.
    /// </summary>
    public enum ConnectionPreferenceType
    {
        CloudOnly = 1,
        LocalPrefered = 2,
        LocalOnly = 3
    }

    /// <summary>
    /// The returned value from a Discovery method call.
    /// Can contain information on both remote and local connections.
    /// </summary>
    public class DiscoveryResponse
    {
        /// <summary>
        /// Gets or sets the remote host identifier for the request
        /// </summary>
        public Guid RemoteHostId { get; set; }

        /// <summary>
        /// Gets or sets the local IP address for the remote host.
        /// </summary>
        public string LocalHostIpAddress { get; set; }

        /// <summary>
        /// Gets or sets an error message for the discovery
        /// </summary>
        public string Error { get; set; }
    }
}
