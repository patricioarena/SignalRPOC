using System;

namespace session_api.CustomException;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string message) : base(message) { }
    public UserNotFoundException() : base("Usuario no encontrado.") { }
}
