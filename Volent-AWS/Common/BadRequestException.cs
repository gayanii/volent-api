using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Volent_AWS
{
    public class BadRequestException : AppException
    {
        public BadRequestException(
           string errorCode = "bad_request",
           string message = "Bad Request")
           : base(HttpStatusCode.BadRequest, errorCode, message) { }

        public BadRequestException(
            Exception ex,
            string errorCode = "bad_request",
            string message = "Bad Request")
            : base(HttpStatusCode.BadRequest, errorCode, message, ex) { }

        public BadRequestException(
            int apiErrorCode,
            string errorCode = "bad_request",
            string message = "Bad Request")
            : base(HttpStatusCode.BadRequest, apiErrorCode, errorCode, message) { }
    }

    public abstract class AppException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public string ErrorCode { get; } = null;

        public int? APIErrorCode { get; } = null;

        public AppException(HttpStatusCode statusCode, Exception ex)
            : base(ex.Message, ex)
        {
            StatusCode = statusCode;
            ErrorCode = "unknown_error";
        }

        public AppException(HttpStatusCode statusCode, string errorCode, Exception ex)
            : base(ex.Message, ex)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }

        public AppException(HttpStatusCode statusCode, string errorCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }

        public AppException(HttpStatusCode statusCode, string errorCode, string message, Exception ex)
            : base(message, ex)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }

        public AppException(HttpStatusCode statusCode, int apiErrorCode, string errorCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
            APIErrorCode = apiErrorCode;
        }

        public AppException(HttpStatusCode statusCode, int apiErrorCode, string errorCode, string message, Exception ex)
            : base(message, ex)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
            APIErrorCode = apiErrorCode;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}{2}", ErrorCode, Environment.NewLine, base.ToString());
        }
    }
}
