using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himsa.Noah.MobileAccessLayer
{
    /// <summary>
    /// A session. Mainly used to bundle <see cref="Patient"/>s.
    /// </summary>
    public class Session
    {
        /// <summary>
        /// The ID of the Session
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The date of Creation. (Format yyyy-MM-ddTHH:mm:ssZ)")]
        /// </summary>
        public string CreateDate { get; set; }
    }
}
