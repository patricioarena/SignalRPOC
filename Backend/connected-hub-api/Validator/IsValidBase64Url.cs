namespace connected_hub_api.Validator;

public class IsValidBase64Url
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
    public static bool Test(string input)
    {
        if (string.IsNullOrEmpty(input)) { return false; }

        // Usar expresión regular para validar la Base64 URL
        return System.Text.RegularExpressions.Regex.IsMatch(input, Base64UrlPattern);
    }
}
