using System;

namespace Himsa.Noah.MobileAccessLayer
{
    /// <summary>
    /// Exception native to the Access Layer
    /// </summary>
    public class AccessLayerException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public AccessLayerException(string message = null, Exception innerException = null)
            : base(message, innerException)
        {}
    }

    /// <summary>
    /// Used to handle server-side exceptions.
    /// </summary>
    public class AccessLayerNoahMobileException : AccessLayerException
    {
        public NoahMobileFault Fault { get; private set; }
        public new string StackTrace { get; private set; }
        public AccessLayerNoahMobileException(NoahMobileFault fault)
        {
            StackTrace = fault.StackTrace;
            Fault = fault;
        }

        public override string Message
        {
            get { return Fault.Message; }
        }

        public override string ToString()
        {
            return Fault.ToString();
        }
    }

    

    /// <summary>
    /// Exception native to the Access Layer, with an Object attached.
    /// Used for attaching an object to an exception.
    /// </summary>
    public class AccessLayerExceptionWithObject : AccessLayerException
    {
        /// <summary>
        /// The Object attached to the exception.
        /// </summary>
        public object Object { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="Object"></param>
        /// <param name="innerException"></param>
        public AccessLayerExceptionWithObject(string message = null, object Object = null,
            Exception innerException = null)
            : base(message, innerException)
        {
            this.Object = Object;
        }

        /// <summary>
        /// Overrides the common ToString(...) in order to add the attached Object.
        /// </summary>
        public override string ToString()
        {
            return base.ToString() + Environment.NewLine + "Object: " + Object.ToString();
        }
    }

}
