using System;
using System.Net;

namespace session_api.Result
{
    public class Response<T> where T : class
    {
        private object data1;

        public Response(T data = null)
        {
            this.statusCode = HttpStatusCode.OK;
            this.data = data;
            this.message = null;
        }

        public Response(HttpStatusCode statusCode, String message = null, T data = null, String developerMessage = null)
        {
            this.statusCode = statusCode;
            this.data = data;
            this.message = message;
            this.developerMessage = developerMessage;
        }

        public Response(HttpStatusCode statusCode, String message = null, T data = null, String developerMessage = null, int errorCode = 0)
        {
            this.statusCode = statusCode;
            this.data = data;
            this.message = message;
            this.developerMessage = developerMessage;
            this.errorCode = errorCode;
        }

        public Response(Exception e)
        {
            this.data = null;
            if (e is CustomException)
            {
                this.statusCode = HttpStatusCode.PreconditionFailed;
                this.message = "ha ocurrido un error de aplicacion";
                this.data = null;
                this.errorCode = ((CustomException)e).ErrorCode;
                this.developerMessage = ((CustomException)e).Message;
            }
            else
            {
                this.statusCode = HttpStatusCode.InternalServerError;
                this.message = "ha ocurrido un error no controlado";
                if ((e.InnerException != null) && (e.InnerException.Message != null))
                    this.developerMessage = e.InnerException.Message;
                else
                    this.developerMessage = e.Message;
            }
        }

        public Response(HttpStatusCode statusCode, string message, object data1)
        {
            this.statusCode = statusCode;
            this.message = message;
            this.data1 = data1;
        }

        public HttpStatusCode statusCode { get; set; }
        public String message { get; set; }
        public T data { get; set; }
        public String developerMessage { get; set; }
        public int errorCode { get; set; }
    }
}

