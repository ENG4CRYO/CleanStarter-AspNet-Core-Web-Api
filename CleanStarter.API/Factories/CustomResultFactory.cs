using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;
using CleanStarter.Application.Common;
using FluentValidation.Results;

namespace CleanStarter.api.Factories
{
   
    public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
    {
        public async Task<IActionResult?> CreateActionResult(ActionExecutingContext context,
            ValidationProblemDetails validationProblemDetails,
            IDictionary<FluentValidation.IValidationContext,
                ValidationResult> validationResults)
        {
            var errors = context.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();


            var response = ApiResponse<object>.Failure("", errors);


            return new BadRequestObjectResult(response);
        }
    }
}