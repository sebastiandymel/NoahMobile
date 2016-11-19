using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himsa.Noah.MobileAccessLayer
{
    /// <summary>
    /// Describes permissions for the properties of an object
    /// </summary>
    public class NoahServerSettings
    {
        /// <summary>
        /// Are edit of old actions allowed.
        /// </summary>
        public bool EditOldActionsAllowed { get; set; }
        /// <summary>
        /// The Version of the connected Noah Server
        /// </summary>
        public string NoahVersion { get; set; }
        /// <summary>
        /// Are Patient Management allowed
        /// </summary>
        public bool AllowPatientManagement { get; set; }
        /// <summary>
        /// Are HIPAA logging enabled. (AuditTrail enabled or not in Noah)
        /// </summary>
        public bool HIPAALogEnabled { get; set; }
    }
}