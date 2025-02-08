using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace eCommerceApp.Infrastructure.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate _next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DbException ex)
        {
            if (ex.InnerException is SqlException innerException)
            {
                switch (innerException.Number)
                {
                    case 2627: // unique constraint violation
                        context.Response.StatusCode = StatusCodes.Status409Conflict;
                        await context.Response.WriteAsync("Unique constraint violation");
                        break;
                    
                    case 515: //cannot insert null
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Cannot insert null");
                        break;

                    case 547: //Foreign key constraint violation
                        context.Response.StatusCode= StatusCodes.Status409Conflict;
                        await context.Response.WriteAsync("Foreign key constraint violation");
                        break;

                }
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("An error occured while saving the entity changes.");
            }

        }
        catch (Exception ex)
        {
            context.Response.StatusCode= StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("An error occured:"+ex.Message);
        }
    }
}
