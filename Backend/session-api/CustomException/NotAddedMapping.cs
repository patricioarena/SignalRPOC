using System;

namespace session_api.CustomException;

public class NotAddedMapping : Exception
{
    public NotAddedMapping(string message) : base(message) { }
    public NotAddedMapping(): base("Mapeo no agregada.") { }
}
