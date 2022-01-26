using System;
using System.Runtime.Serialization;

namespace Volent_AWS.Manager
{
    [Serializable]
    internal class HttpResponseException : Exception
    {
        private object badRequest;

        public HttpResponseException()
        {
        }

        public HttpResponseException(object badRequest)
        {
            this.badRequest = badRequest;
        }

        public HttpResponseException(string message) : base(message)
        {
        }

        public HttpResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HttpResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}