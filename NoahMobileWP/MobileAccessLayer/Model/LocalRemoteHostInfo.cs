using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himsa.Noah.MobileAccessLayer
{
    public class LocalRemoteHostInfo
    {
        /// <summary>
        /// Gets or sets the unique identifier for the remote host.
        /// </summary>
        public Guid RemoteHostId { get; set; }

        /// <summary>
        /// The local IP address for the remote host.
        /// </summary>
        public string LocalHostIpAddress { get; set; }

        /// <summary>
        /// Gets or sets the date the object was created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the date the object was last updated.
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// Host name
        /// </summary>
        public String HostName { get; set; }

        /// <summary>
        /// Friendly name
        /// </summary>
        public String FriendlyName { get; set; }

        /// <summary>
        /// Local Noah server guid used for server id when broadcasting for available Noah server for Noah mobile
        /// </summary>
        public String LocalNoahServerGuid { get; set; }


    }
}
