using Microsoft.AspNetCore.Mvc.Filters;
using PresenceBackend.Controllers.v1;
using PresenceBackend.Services;
using PresenceBackend.Shared;

namespace PresenceBackend.Filters;

public class AuthorizationFilter : ActionFilterAttribute
{

    private readonly DbAccess db;
    private readonly IAuthorization auth;

    public AuthorizationFilter(DbAccess db, IAuthorization auth)
    {
        this.db = db;
        this.auth = auth;
    }
    
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var controller = context.Controller as AuthorizedControllerBase;
        if (controller == null)
            throw new Exception("the controller is not an AuthorizedController");

        if (!context.HttpContext.Request.Headers.TryGetValue("authorization", out var token))
        {
            context.Result = controller.Unauthorized();
            return;
        }

        try
        {
            controller.CurrentUser = await auth.ValidateAccessToken(token!);
        } catch (Exception e)
        {
            Console.WriteLine(e.Message);
            context.Result = controller.Unauthorized();
            return;
        }

        if (controller.CurrentUser == null)
        {
            context.Result = controller.Unauthorized();
            return;
        }

        await next();
    }

}