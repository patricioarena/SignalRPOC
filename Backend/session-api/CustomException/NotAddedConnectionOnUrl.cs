using System;

namespace session_api.CustomException;

public class NotAddedConnectionOnUrl : Exception
{
    public NotAddedConnectionOnUrl(string message) : base(message) { }
    public NotAddedConnectionOnUrl() : base("Conexión no agregada.") { }
}
