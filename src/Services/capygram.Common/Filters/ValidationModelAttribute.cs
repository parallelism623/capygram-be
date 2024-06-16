using capygram.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;

namespace capygram.Common.Filters
{
    public class ValidationModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var viewModel = context.Controller.ViewData;
            if (!viewModel.ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = viewModel.ModelState.Values.SelectMany(v => v.Errors);
                throw new BadRequestException(allErrors.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
