using Store.G02.Domain.Exceptions;
using Store.G02.Shared.ErrorsModels;

namespace Store.G02.Web.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try 
            {
                await _next.Invoke(context);
                if(context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    await HandlingNotFoundEndPointAsync(context);
                }

            }   
            catch (Exception ex)
            {
                // Log Exception
                _logger.LogError(ex, ex.Message);

                await HJandlingErrorAsync(context, ex);

            }
        }

        private static async Task HJandlingErrorAsync(HttpContext context, Exception ex)
        {
            // 1. Set Status Code For Response
            // 2. Set Content Type Code For Response
            // 3. Response Object (Body)
            // 4. Return Response 

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new ErrorDetails()
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorMessage = ex.Message
            };

            response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                UnAuthorizedException => StatusCodes.Status401Unauthorized,
                ValidationException => HandleValidationExceptionAsync((ValidationException)ex, response),
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = response.StatusCode;

            await context.Response.WriteAsJsonAsync(response);
        }

        private static async Task HandlingNotFoundEndPointAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorDetails()
            {
                StatusCode = StatusCodes.Status404NotFound,
                ErrorMessage = $"End Point {context.Request.Path} is Not Found"
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        
        private static int  HandleValidationExceptionAsync(ValidationException ex, ErrorDetails response)
        {
           response.Errors = ex.Errors;
            return StatusCodes.Status400BadRequest;
        }
    }
}
