using System;
using System.Runtime.Serialization;

namespace ConsoleServer.Logic
{
    [Serializable]
    internal class SocketClientException : Exception
    {
        public SocketClientException()
        {
        }

        public SocketClientException(string message) : base(message)
        {
        }

        public SocketClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SocketClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}