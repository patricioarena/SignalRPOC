using System.Text.RegularExpressions;
using System.Web;

namespace connected_hub_api.Validator;

public class IsValidUrl
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
    public static bool Test(string input)
    {
        if (string.IsNullOrEmpty(input)) { return false; }

        // Si la URL contiene codificación en porcentaje, decodificarla
        if (Regex.IsMatch(input, PercentEncodedPattern)) { input = HttpUtility.UrlDecode(input); }

        // Usar expresión regular para validar que la URL (decodificada o no) comienza con 'http://' o 'https://'
        return Regex.IsMatch(input, UrlPattern, RegexOptions.IgnoreCase);
    }
}
