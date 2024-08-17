using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace session_api.Service;

public static class Encode
{
    // Expresión regular para validar URL que comienzan con http o https
    private static readonly string UrlPattern = @"^https?:\/\/";

    // Expresión regular para verificar si la URL contiene codificación en porcentaje
    private static readonly string PercentEncodedPattern = @"%[0-9A-Fa-f]{2}";

    /// <summary>
    /// Valida si la cadena de entrada es una URL válida para la codificación en Base64 URL.
    ///
    /// Verifica que la URL, después de decodificar el formato de codificación de porcentaje, comience con 'http://' o 'https://'.
    ///
    /// </summary>
    /// <param name="input">La cadena a validar.</param>
    /// <returns>Verdadero si la URL es válida; de lo contrario, falso.</returns>
    public static bool IsValidUrl(string input)
    {
        if (string.IsNullOrEmpty(input)) { return false; }

        // Si la URL contiene codificación en porcentaje, decodificarla
        if (Regex.IsMatch(input, PercentEncodedPattern)) { input = HttpUtility.UrlDecode(input); }

        // Usar expresión regular para validar que la URL (decodificada o no) comienza con 'http://' o 'https://'
        return Regex.IsMatch(input, UrlPattern, RegexOptions.IgnoreCase);
    }

    /// <summary>
    /// Codifica una cadena en Base64 URL según la especificación RFC 4648.
    ///
    /// La codificación Base64 URL es una variante de la codificación Base64 estándar
    /// definida en RFC 4648. A diferencia de la Base64 estándar, esta variante reemplaza
    /// los caracteres `+` y `/` con `-` y `_` respectivamente, y omite el carácter de padding
    /// `=`. Esta codificación es útil en contextos donde la Base64 debe ser compatible con URLs
    /// y sistemas de archivos, como en tokens JWT y ciertos esquemas de codificación de datos en la web.
    ///
    /// Ventajas:
    /// - **Compatibilidad con URLs:** La variante URL es segura para ser utilizada en URLs
    ///   y nombres de archivos, evitando caracteres que pueden tener significados especiales
    ///   en estos contextos.
    /// - **Robustez:** Se ajusta bien a los estándares modernos para la codificación de datos.
    ///
    /// Desventajas:
    /// - **Legibilidad:** La cadena codificada no es legible para los humanos y puede ser más
    ///   larga que la cadena original.
    ///
    /// </summary>
    /// <param name="input">La cadena que se va a codificar.</param>
    /// <returns>La cadena codificada en Base64 URL.</returns>
    public static string Base64Url(string input)
    {
        if (!IsValidUrl(input)) { return null; }

        try
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            string base64 = Convert.ToBase64String(bytes);

            return base64
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
