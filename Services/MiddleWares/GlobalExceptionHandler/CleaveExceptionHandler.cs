using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GlobalExceptionHandler.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;


namespace Cleave.Middleware.ExceptionHandler
{
    public class CleaveExceptionHandler : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                ExecutionDetails executionDetails = BuildExecutionDetailsObjectFromException(ex);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.WriteAsync(JsonSerializer.Serialize(executionDetails));
            }
        }

        internal ExecutionDetails BuildExecutionDetailsObjectFromException(Exception ex)
        {
            ErrorDetails error = new ErrorDetails()
            {
                ErrorMessage = ex.Message,
                StackTrace = ex.StackTrace
            };
            ExecutionDetails executionDetails = new ExecutionDetails()
            {
                IsSuccess = false,
                Error = error
            };
            return executionDetails;
        }
    }

    public static class CleaveExceptionHandlerMiddlewareExtension
    {
        public static IApplicationBuilder UseCleaveExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CleaveExceptionHandler>();
        }

        public static IServiceCollection AddCleaveExceptionHandler(this IServiceCollection services)
        {
            return services.AddSingleton<CleaveExceptionHandler>();
        }
    }
}
