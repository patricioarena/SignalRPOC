namespace connected_hub_api.Result
{
    using System.Net;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Defines the <see cref="Response{D, M}" />
    /// </summary>
    /// <typeparam name="D"></typeparam>
    /// <typeparam name="M"></typeparam>
    public class Response<D, M> where D : class where M : class
    {
        /// <summary>
        /// Gets the StatusCode
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// Gets the Message
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; private set; }

        /// <summary>
        /// Gets the Data
        /// </summary>
        public D? Data { get; private set; }

        /// <summary>
        /// Gets the Metadata
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public M? Metadata { get; private set; }

        /// <summary>
        /// Gets the DeveloperMessage
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DeveloperMessage { get; private set; }

        /// <summary>
        /// Gets the ErrorCode
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? ErrorCode { get; private set; }

        /// <summary>
        /// Prevents a default instance of the <see cref="Response{D, M}"/> class from being created.
        /// </summary>
        private Response()
        {
        }

        // Método estático para inicializar el DataBuilder sin usar new

        /// <summary>
        /// The Builder
        /// </summary>
        /// <returns>The <see cref="Builder"/></returns>
        public static DataBuilder Builder() => new DataBuilder();

        /// <summary>
        /// Defines the <see cref="DataBuilder" />
        /// </summary>
        public class DataBuilder
        {
            /// <summary>
            /// Defines the _statusCode
            /// </summary>
            private HttpStatusCode _statusCode = HttpStatusCode.OK;

            /// <summary>
            /// Defines the _message
            /// </summary>
            private string? _message;

            /// <summary>
            /// Defines the _data
            /// </summary>
            private D? _data;

            /// <summary>
            /// Defines the _metadata
            /// </summary>
            private M? _metadata;

            /// <summary>
            /// Defines the _developerMessage
            /// </summary>
            private string? _developerMessage;

            /// <summary>
            /// Defines the _errorCode
            /// </summary>
            private int? _errorCode;

            // Constructor privado para evitar acceso externo

            /// <summary>
            /// Initializes a new instance of the <see cref="DataBuilder"/> class.
            /// </summary>
            internal DataBuilder() { }

            /// <summary>
            /// The SetStatusCode
            /// </summary>
            /// <param name="statusCode">The statusCode<see cref="HttpStatusCode"/></param>
            /// <returns>The <see cref="DataBuilder"/></returns>
            public DataBuilder SetStatusCode(HttpStatusCode statusCode)
            {
                _statusCode = statusCode;
                return this;
            }

            /// <summary>
            /// The SetMessage
            /// </summary>
            /// <param name="message">The message<see cref="string"/></param>
            /// <returns>The <see cref="DataBuilder"/></returns>
            public DataBuilder SetMessage(string message)
            {
                _message = message;
                return this;
            }

            /// <summary>
            /// The SetData
            /// </summary>
            /// <param name="data">The data<see cref="D"/></param>
            /// <returns>The <see cref="DataBuilder"/></returns>
            public DataBuilder SetData(D data)
            {
                _data = data;
                return this;
            }

            /// <summary>
            /// The SetMetadata
            /// </summary>
            /// <param name="metadata">The metadata<see cref="M"/></param>
            /// <returns>The <see cref="DataBuilder"/></returns>
            public DataBuilder SetMetadata(M metadata)
            {
                _metadata = metadata;
                return this;
            }

            /// <summary>
            /// The SetDeveloperMessage
            /// </summary>
            /// <param name="developerMessage">The developerMessage<see cref="string"/></param>
            /// <returns>The <see cref="DataBuilder"/></returns>
            public DataBuilder SetDeveloperMessage(string developerMessage)
            {
                _developerMessage = developerMessage;
                return this;
            }

            /// <summary>
            /// The SetErrorCode
            /// </summary>
            /// <param name="errorCode">The errorCode<see cref="int"/></param>
            /// <returns>The <see cref="DataBuilder"/></returns>
            public DataBuilder SetErrorCode(int errorCode)
            {
                _errorCode = errorCode;
                return this;
            }

            /// <summary>
            /// The Build
            /// </summary>
            /// <returns>The <see cref="Response{D, M}"/></returns>
            public Response<D, M> Build()
            {
                return new Response<D, M>
                {
                    StatusCode = _statusCode,
                    Message = _message,
                    Data = _data,
                    Metadata = _metadata,
                    DeveloperMessage = _developerMessage,
                    ErrorCode = _errorCode
                };
            }
        }
    }
}
