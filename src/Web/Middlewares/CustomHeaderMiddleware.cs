namespace CleanArchitectureProject.Web.Middlewares;

public class CustomHeaderMiddleware
{
    private readonly RequestDelegate _next;

    public CustomHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Response.Headers["X-Powered-By"] = "CleanArchitecture";
        await _next(context);
    }
}
