using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Alinta.Core;
using Alinta.Services.Abstractions.Responses;
using Alinta.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Alinta.WebApi.Presenters
{
    public class CreateCustomerResponsePresenter : IOperationResultPresenter<CreateCustomerResponse>
    {
        public IActionResult Handle(OperationResult<CreateCustomerResponse> result)
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

            return new ObjectResult(result.Message) {StatusCode = (int) HttpStatusCode.InternalServerError};

        }
    }
}
