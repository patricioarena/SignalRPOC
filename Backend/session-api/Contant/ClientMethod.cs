namespace session_api.Contant;

/// <summary>
/// Mensajes que el cliente puede recibir para ejecutar una operación
/// </summary>
public static class ClientMethod
{
    public const string Welcome = "welcome";
    public const string Show_Notification = "show_notification";
    public const string Received_Data = "received_data";
    public const string Validation_Error = "validation_error";
}
