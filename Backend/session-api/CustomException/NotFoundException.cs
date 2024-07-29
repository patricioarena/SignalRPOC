using System;

namespace session_api.CustomException;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
    public NotFoundException() : base("No encontrado.") { }
}
