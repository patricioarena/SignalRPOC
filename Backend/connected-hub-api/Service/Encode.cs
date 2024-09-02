using connected_hub_api.Validator;
using System;
using System.Text;

namespace connected_hub_api.Service;

public static class Encode
{
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
        if (!IsValidUrl.Test(input)) { return null; }

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
