using System;
using System.Linq;
using System.Net;
using Alinta.Core;
using Alinta.Services.Abstractions.Responses;
using Alinta.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Alinta.WebApi.Presenters
{
    public class SearchCustomersResponsePresenter : IOperationResultPresenter<SearchCustomersResponse>
    {
        public IActionResult Handle(OperationResult<SearchCustomersResponse> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (result.Status)
            {
                if (!result.Data.Customers.Any())
                {
                    return new NotFoundObjectResult("Sorry there are no customers matching the filter criteria");
                }

                var displayDto = result.Data.Customers.Select(x => x.ToDisplayDto());
                return new OkObjectResult(displayDto);
            }

            return new ObjectResult(result.Message) { StatusCode = (int)HttpStatusCode.InternalServerError };

        }
    }
}