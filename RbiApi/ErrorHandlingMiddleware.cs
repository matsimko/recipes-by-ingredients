using RbiData.Services;
using System.Net;
using System.Text.Json;

namespace RbiApi;

//https://uncommonbytes.com/blog/2020/04/12/global-exception-handling-in-aspnet-core-api/
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;

    public ErrorHandlingMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode status;
        string message = exception.Message;
        var stackTrace = String.Empty;

        if (exception is EntityNotFoundException)
        {
            status = HttpStatusCode.NotFound;
        }
        else if (exception is OwnershipException)
        {
            status = HttpStatusCode.Forbidden;
        }
        else
        {
            status = HttpStatusCode.InternalServerError;
            if (_env.IsEnvironment("Development"))
                stackTrace = exception.ToString();//exception.StackTrace;
        }

        var result = JsonSerializer.Serialize(new { error = message, stackTrace });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;
        return context.Response.WriteAsync(result);
    }
}
