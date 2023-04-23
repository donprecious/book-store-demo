using System;
using System.Collections.Generic;
using BookStore.Application.Common.Exceptions;
using BookStore.Application.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace WebApi.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        public ApiExceptionFilterAttribute()
        {
            // Register known exception types and handlers.
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
                { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
                { typeof(BadRequestException), HandleBadRequestException },
            };
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);

            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            Type type = context.Exception.GetType();
            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }

            if (!context.ModelState.IsValid)
            {
                HandleInvalidModelStateException(context);
                return;
            }

            HandleUnknownException(context);
        }

        private void HandleValidationException(ExceptionContext context)
        {
            var exception = (ValidationException)context.Exception;

            // var details = new ValidationProblemDetails(exception.Errors)
            // {
            //     Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            // };
             var errors = exception.Errors.Values.SelectMany(a=>a);
           var result = Result.Failure(errors);
            context.Result = new BadRequestObjectResult(result);

            context.ExceptionHandled = true;
        }

        private void HandleInvalidModelStateException(ExceptionContext context)
        {

            var errors = context.ModelState?.Values.SelectMany(a => a?.Errors).Select(a => a?.ErrorMessage);
            var result = Result.Failure(errors, "one or more errors occured");
            context.Result = new BadRequestObjectResult(result);

            context.ExceptionHandled = true;
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            var exception = (NotFoundException)context.Exception;

         
            var result = Result.Failure(Array.Empty<string>(), exception?.Message);
            context.Result = new NotFoundObjectResult(result);

            context.ExceptionHandled = true;
        }
        
        private void HandleBadRequestException(ExceptionContext context)
        {
            var exception = (BadRequestException)context.Exception;

         
            var result = Result.Failure(Array.Empty<string>(), exception?.Message);
            context.Result = new BadRequestObjectResult(result);
      
            context.ExceptionHandled = true;
        }

        private void HandleUnauthorizedAccessException(ExceptionContext context)
        {
            
            var result = Result.Failure(Array.Empty<string>(), "Unauthorized");
            context.Result = new ObjectResult(result)
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };

            context.ExceptionHandled = true;
        }

        private void HandleForbiddenAccessException(ExceptionContext context)
        {
           
            var result = Result.Failure(Array.Empty<string>(), "Forbidden");

            context.Result = new ObjectResult(result)
            {
                StatusCode = StatusCodes.Status403Forbidden
            };

            context.ExceptionHandled = true;
        }

        private void HandleUnknownException(ExceptionContext context)
        {
          
            var result = Result.Failure(Array.Empty<string>(), "An error occurred while processing your request.");
            context.Result = new ObjectResult(result)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}