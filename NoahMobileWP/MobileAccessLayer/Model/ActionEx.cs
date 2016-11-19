using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himsa.Noah.MobileAccessLayer
{
    /// <summary>
    /// Holds data regarding an action.
    /// </summary>
    public class ActionEx : Action
    {
        /// <summary>
        /// The ID of the PatientAction.
        /// </summary>
        public int PatientId { get; set; }
    }
}
