using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himsa.Noah.MobileAccessLayer
{
    public enum MobileAppType
    {
        /// <summary>
        /// App type to use if the app can fit hearing aid
        /// </summary>
        FittingApp = 1,
        /// <summary>
        /// App type to use if app can do measurements e.g. audiograms, REM
        /// </summary>
        MeasurementApp = 2,
        /// <summary>
        /// App type to use if your app is of a more general nature e.g. handling jounal records
        /// </summary>
        GeneralApp = 3
    }
    /// <summary>
    /// Information about an applications registration on Noah Mobile.
    /// </summary>
    public class AppRegInfo
    {
        /// <summary>
        /// Gets or sets the Version of the app.
        /// The version will may be displayed in the configuration settings of the Business System.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the app name.
        /// The Name will may be displayed in the configuration settings of the Business System.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ModuleId.
        /// This id can be used to display the ModuleId of the app in the configuration settings of the Business System.
        /// </summary>
        public int ModuleId { get; set; }
        /// <summary>
        /// Get or set the app type
        /// </summary>
        public string MobileAppType { get; set; }
    }
}
