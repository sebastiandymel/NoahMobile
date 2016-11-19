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
    public class AppPermission
    {
        /// <summary>
        /// The ID that identifies the permission
        /// </summary>
        public int PermissionID { get; set; }
        /// <summary>
        /// The name of the property that this permission relates to
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The C# type of the property
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// Specify whether or not this field is required when creating/changing a record
        /// </summary>       
        public bool Required { get; set; }
        /// <summary>                 
        /// Is the permission a default permission i.e. part of the permission that a new app gets when it register with Noah
        /// </summary>
        public bool Default { get; set; }
        /// <summary>
        /// Minimum length of the field (only valid for strings)
        /// </summary>
        public int MinLength { get; set; }
        /// <summary>
        /// Maximum length of the field (only valid for strings)
        /// </summary>
        public int MaxLength { get; set; }
    }
}