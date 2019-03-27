using System;
using System.Net;
using Alinta.Core;
using Alinta.Services.Abstractions.Responses;
using Alinta.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Alinta.WebApi.Presenters
{
    public class UpdateCustomerResponsePresenter : IOperationResultPresenter<UpdateCustomerResponse>
    {
        public IActionResult Handle(OperationResult<UpdateCustomerResponse> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (result.Status)
            {
                var displayDto = result.Data.Customer.ToDisplayDto();
                return new OkObjectResult(displayDto);
            }

            return new ObjectResult(result.Message) { StatusCode = (int)HttpStatusCode.InternalServerError };

        }
    }
}