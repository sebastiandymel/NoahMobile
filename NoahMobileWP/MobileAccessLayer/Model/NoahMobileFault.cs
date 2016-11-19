namespace Himsa.Noah.MobileAccessLayer
{
    public enum FaultOrigin
    {
        NoahServer = 0,
        Cloud = 1,
    }

    public class NoahMobileFault
    {
        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        /// <value>
        /// The stack trace.
        /// </value>
        public string StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the origin.
        /// </summary>
        /// <value>
        /// The origin.
        /// </value>
        public FaultOrigin Origin { get; set; }

        public override string ToString()
        {
            return string.Format("({3})Error Code: {0}, Message: {1}\r\nAt:\r\n{2}", ErrorCode, Message, StackTrace, Origin);
        }
    }
}
