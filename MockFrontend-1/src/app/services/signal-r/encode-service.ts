import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class EncodeService {
  /**
   * Codifica una cadena en Base64 URL según la especificación RFC 4648.
   *
   * La codificación Base64 URL es una variante de la codificación Base64 estándar
   * definida en RFC 4648. A diferencia de la Base64 estándar, esta variante reemplaza
   * los caracteres `+` y `/` con `-` y `_` respectivamente, y omite el carácter de padding
   * `=`. Esta codificación es útil en contextos donde la Base64 debe ser compatible con URLs
   * y sistemas de archivos, como en tokens JWT y ciertos esquemas de codificación de datos en la web.
   *
   * Ventajas:
   * - **Compatibilidad con URLs:** La variante URL es segura para ser utilizada en URLs
   *   y nombres de archivos, evitando caracteres que pueden tener significados especiales
   *   en estos contextos.
   * - **Robustez:** Se ajusta bien a los estándares modernos para la codificación de datos.
   *
   * Desventajas:
   * - **Legibilidad:** La cadena codificada no es legible para los humanos y puede ser más
   *   larga que la cadena original.
   *
   * @param input La cadena que se va a codificar.
   * @returns La cadena codificada en Base64 URL.
   */

  public base64Url(input: string): string {
    // Codificar la cadena en Base64 estándar utilizando TextEncoder y ArrayBuffer
    const bytes = new TextEncoder().encode(input);

     // Convertir los bytes a una cadena binaria
    const binaryArray = Array.from(bytes, byte => String.fromCharCode(byte)).join('');

    // Codificar la cadena binaria en Base64
    const base64 = window.btoa(binaryArray);

    // Convertir la cadena Base64 a URL Safe Base64
    return base64
      .replace(/\+/g, '-')
      .replace(/\//g, '_')
      .replace(/=+$/, '');
  }
}
