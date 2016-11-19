using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himsa.Noah.MobileAccessLayer
{
    /// <summary>
    /// Holds filtering options regarding action searches.
    /// </summary>
    public class ActionFilter
    {
        // Specify which actions is returned e.g. all or only the first. All = 0 (default), Latest, = 1 (newest with respect to create day"
        public int ActionsToReturn { get; set; }

        // Returns all actions which are created with one of the module ids in the given array. If the array is empty no filtering on module id is applied")]
        public int[] ModuleIDs { get; set; }

        // Returns all actions which are created with the ManufacturerID. If -1 no filtering on manufacturerID id is applied.")]
        public int ManufacturerID { get; set; }

        // Returns all actions of the given data type. If -1 no filtering on data type is applied.")]
        public int DataType { get; set; }

        // Return all actions in the given action group. If string is empty no filtering on action group is applied. The Actiongroup to be found. (Format yyyy-MM-ddTHH:mm:ssZ)")]
        public string ActionGroup { get; set; }

        public static ActionFilter CreateNeutralFilter()
        {
            return new ActionFilter()
            {
                ActionsToReturn = 0,
                ModuleIDs = new int[0],
                ManufacturerID = -1,
                DataType = -1,
                ActionGroup = ""
            };
        }
    }
}
