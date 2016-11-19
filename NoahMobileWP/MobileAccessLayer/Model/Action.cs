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
    public class Action
    {
        /// <summary>
        /// The ID of the Action.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The session that the action belongs to.
        /// </summary>
        public int SessionID { get; set; }

        /// <summary>
        /// The date of Creation. (Format yyyy-MM-ddTHH:mm:ssZ).
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// The ID of the module that created the Action. (Is composed as Hi word ManufactuerID and low word ModuleID).
        /// </summary>
        public int ModuleID { get; set; }

        /// <summary>
        /// The Modules description of the Action (min. 1, max. 64 characters).
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The DataType of the Action.
        /// </summary>
        public int DataTypeCode { get; set; }

        /// <summary>
        /// The Standard format for the Public Data.
        /// </summary>
        public int DataFmtStd { get; set; }

        /// <summary>
        /// A module specific value. (Could be format of private data).
        /// </summary>
        public int DataFmtExt { get; set; }

        /// <summary>
        /// The datetime of the ActionGroup that the action belongs to. (Format yyyy-MM-ddTHH:mm:ssZ) (Empty string is no actiongroup).
        /// </summary>
        public string ActionGroup { get; set; }

        /// <summary>
        /// DeviceType of the action.
        /// </summary>
        public int DeviceType { get; set; }

        /// <summary>
        /// The date when the action was last modified. (Format yyyy-MM-ddTHH:mm:ssZ).
        /// </summary>
        public string LastModifiedDate { get; set; }

        /// <summary>
        /// UserInitials of the user that created the action. (max 3 characters) ??? if not found.
        /// (Optional)
        /// </summary>
        public string UserInitials { get; set; }
    }
}
