using System;
using System.Text;

namespace session_api.Service;

public static class Decode
{
    private static readonly string Base64UrlPattern = @"^[A-Za-z0-9_-]+$";

    /// <summary>
    /// Valida si la cadena de entrada es una cadena codificada en Base64 URL válida.
    ///
    /// La Base64 URL debe contener solo caracteres alfanuméricos, `-` y `_`.
    ///
    /// </summary>
    /// <param name="input">La cadena a validar.</param>
    /// <returns>Verdadero si la cadena es una Base64 URL válida; de lo contrario, falso.</returns>
    public static bool IsValidBase64Url(string input)
    {
        if (string.IsNullOrEmpty(input)) { return false; }

        // Usar expresión regular para validar la Base64 URL
        return System.Text.RegularExpressions.Regex.IsMatch(input, Base64UrlPattern);
    }

    /// <summary>
    /// Decodifica una cadena codificada en Base64 URL según la especificación RFC 4648.
    ///
    /// La codificación Base64 URL es una variante de la codificación Base64 estándar
    /// definida en RFC 4648. A diferencia de la Base64 estándar, esta variante reemplaza
    /// los caracteres `+` y `/` con `-` y `_` respectivamente, y omite el carácter de padding
    /// `=`. Esta codificación es utilizada comúnmente en contextos donde la Base64 debe ser
    /// compatible con URLs y sistemas de archivos, como en tokens JWT y ciertos esquemas de
    /// codificación de datos en la web.
    ///
    /// Ventajas:
    /// - **Compatibilidad con URLs:** Los caracteres especiales de la Base64 URL son seguros
    ///   para ser utilizados en URLs y nombres de archivos, evitando problemas con caracteres
    ///   que tienen significados especiales en estos contextos.
    /// - **Robustez:** Se ajusta a los estándares modernos para la codificación de datos.
    ///
    /// Desventajas:
    /// - **Legibilidad:** La cadena codificada en Base64 URL no es legible para los humanos y
    ///   puede ser más larga que la cadena original.
    ///
    /// </summary>
    /// <param name="input">La cadena codificada en Base64 URL a decodificar.</param>
    /// <returns>La cadena decodificada.</returns>
    public static string Base64Url(string input)
    {
        if (!IsValidBase64Url(input)) { return null; }

        try
        {
            string base64 = input
                .Replace('-', '+')
                .Replace('_', '/');

            base64 = base64.PadRight(base64.Length + ((4 - (base64.Length % 4)) % 4), '=');

            byte[] bytes = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(bytes);
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
