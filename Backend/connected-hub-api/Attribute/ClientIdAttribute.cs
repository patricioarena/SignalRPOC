using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

/// <summary>
/// Atributo personalizado para validar la presencia de un ClientId en los encabezados de la solicitud HTTP.
/// Este atributo se puede aplicar a controladores o métodos de acción específicos para asegurar que la solicitud
/// contenga un ClientId válido antes de ejecutar la acción.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ClientIdAttribute : ActionFilterAttribute
{
    /// <summary>
    /// Obtiene o establece un valor que indica si la validación del ConnectionId está habilitada.
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Método sobrescrito que se ejecuta antes de que la acción del controlador se ejecute.
    /// Valida si la solicitud HTTP contiene un encabezado ConnectionId cuando la validación está habilitada.
    /// </summary>
    /// <param name="context">El contexto de acción que proporciona acceso a la solicitud y la respuesta HTTP, 
    /// así como a la acción y sus parámetros.</param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Verifica si la validación está habilitada
        if (IsEnabled)
        {
            // Verifica si el encabezado "ConnectionId" está presente en la solicitud
            if (!context.HttpContext.Request.Headers.ContainsKey("ClientId"))
            {
                // Si no está presente, se devuelve un resultado de solicitud incorrecta (400)
                context.Result = new BadRequestObjectResult("ClientId is missing.");
                return;
            }

            if (!int.TryParse(context.HttpContext.Request.Headers["ClientId"], out int result))
            {
                // Si no es un valor valido, se devuelve un resultado de solicitud incorrecta (400)
                context.Result = new BadRequestObjectResult("ClientId is not a valid value.");
                return;
            }

            // Aquí se puede agregar lógica adicional para validar el valor del ClientId si es necesario
        }

        // Llamar al método base para continuar con la ejecución normal si la validación es exitosa o está deshabilitada
        base.OnActionExecuting(context);
    }
}
