namespace Himsa.Noah.MobileAccessLayer
{
    public class UpdatedActionsSearchFilter
    {
        /// <summary>
        /// The start DateTime of the wanted interval. yyyy-MM-ddTHH:mm:ssZ
        /// </summary>
        public string StartDateTime { get; set; }

        /// <summary>
        /// The end DateTime of the wanted interval. yyyy-MM-ddTHH:mm:ssZ
        /// </summary>
        public string EndDateTime { get; set; }

        /// <summary>
        /// The no of page to get.
        /// </summary>
        public string Page { get; set; }

        /// <summary>
        /// The wanted size of the page.
        /// </summary>
        public string PageSize { get; set; }

        /// <summary>
        /// The wanted Datypes.
        /// </summary>
        public int[] DataTypes { get; set; }
    }
}
