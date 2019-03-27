using System;
using System.Net;
using Alinta.Core;
using Alinta.Services.Abstractions.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Alinta.WebApi.Presenters
{
    public class DeleteCustomerResponsePresenter : IOperationResultPresenter<DeleteCustomerResponse>
    {
        public IActionResult Handle(OperationResult<DeleteCustomerResponse> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (result.Status)
            {
                return new OkObjectResult("Customer deleted successfully");
            }

            return new ObjectResult(result.Message) { StatusCode = (int)HttpStatusCode.InternalServerError };

        }
    }
}