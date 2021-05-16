using System;
using System.Runtime.Serialization;

namespace DriveMounter
{
    public class VolumeMountException : Exception
    {
        public VolumeMountException()
        {
        }

        public VolumeMountException(string message) : base(message)
        {
        }

        public VolumeMountException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected VolumeMountException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
