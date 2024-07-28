using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

/// <summary>
/// Filtro para asegurar que todas las rutas en el documento Swagger sean minúsculas.
/// </summary>
public class LowercaseDocumentFilter : IDocumentFilter
{
    /// <summary>
    /// Aplica el filtro al documento Swagger.
    /// </summary>
    /// <param name="swaggerDoc">El documento Swagger.</param>
    /// <param name="context">El contexto del filtro.</param>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var paths = swaggerDoc.Paths.ToDictionary(entry => entry.Key.ToLowerInvariant(), entry => entry.Value);
        swaggerDoc.Paths = new OpenApiPaths();

        foreach (var path in paths)
        {
            swaggerDoc.Paths.Add(path.Key, path.Value);
        }
    }
}   