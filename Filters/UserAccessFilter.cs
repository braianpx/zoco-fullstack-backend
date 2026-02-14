using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Zoco.Api.Extensions; // donde tenés GetUserId y GetUserRole

public class UserAccessFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var user = context.HttpContext.User;

        // Validación básica de autenticación
        if (!user.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userRole = user.GetUserRole();
        var userId = user.GetUserId();

        // Solo aplicamos la regla si hay un argumento llamado "id"
        if (context.ActionArguments.TryGetValue("id", out var argId) && argId is int targetId)
        {
            if (userRole == "User" && targetId != userId)
            {
                context.Result = new ForbidResult();
                return;
            }
        }

        // Si todo ok, seguimos con la acción
        await next();
    }
}
