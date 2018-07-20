using System;
using System.Runtime.Serialization;

namespace RohlikAPI
{
    [Serializable]
    internal class RohlikApiException : Exception
    {
        public RohlikApiException()
        {
        }

        public RohlikApiException(string message) : base(message)
        {
        }

        public RohlikApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RohlikApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}