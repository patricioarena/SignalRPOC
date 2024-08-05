using session_api.Result;
using System;
using System.Net;

namespace session_api.Result
{
    public class Response<T> where T : class
    {
        private object data1;

        public Response(HttpStatusCode ok, String message = null, T data = null)
        {
            this.ok = ok;
            this.data = data;
            this.message = message;
        }

        public Response(HttpStatusCode ok, String message = null, T data = null, String developerMessage = null)
        {
            this.ok = ok;
            this.data = data;
            this.message = message;
            this.developerMessage = developerMessage;
        }

        public Response(Exception e)
        {
            this.data = null;
            if (e is CustomException)
            {
                this.ok = HttpStatusCode.PreconditionFailed;
                this.message = "ha ocurrido un error de aplicacion";
                this.data = null;
                this.errorCode = ((CustomException)e).ErrorCode;
                this.developerMessage = ((CustomException)e).Message;
            }
            else
            {
                this.ok = HttpStatusCode.InternalServerError;
                this.message = "ha ocurrido un error no controlado";
                if ((e.InnerException != null) && (e.InnerException.Message != null))
                    this.developerMessage = e.InnerException.Message;
                else
                    this.developerMessage = e.Message;
            }
        }

        public Response(HttpStatusCode oK, string message, object data1)
        {
            ok = oK;
            this.message = message;
            this.data1 = data1;
        }

        public HttpStatusCode ok { get; set; }
        public String message { get; set; }
        public T data { get; set; }
        public String developerMessage { get; set; }
        public int errorCode { get; set; }
    }
}

