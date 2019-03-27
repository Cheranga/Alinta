using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Alinta.Services.Abstractions.Interfaces;
using Alinta.WebApi.DTO.Requests;
using Alinta.WebApi.DTO.Responses;
using Alinta.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CreateCustomerRequest = Alinta.WebApi.DTO.Requests.CreateCustomerRequest;

namespace Alinta.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]SearchCustomerRequest request)
        {
            var operationResult = await _customerService.SearchCustomersAsync(request.ToServiceRequest()).ConfigureAwait(false);
            if (!operationResult.Status)
            {
                _logger.LogError($"Error: Cannot search for customers");
                return StatusCode((int) HttpStatusCode.InternalServerError, "Cannot search for customers");
            }

            if (!operationResult.Data.Customers.Any())
            {
                return NotFound($"There are no customers matching with : {request.Name}");
            }
            var displayDtos = operationResult.Data.Customers.Select(x => x.ToDisplayDto());
            return Ok(new SearchCustomerResponse(displayDtos));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateCustomerRequest request)
        {
            var operationResult = await _customerService.CreateCustomerAsync(request.ToServiceRequest()).ConfigureAwait(false);
            if (!operationResult.Status)
            {
                _logger.LogError($"Error: Cannot create customer");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Cannot create customer");
            }

            var displayDto = operationResult.Data.Customer.ToDisplayDto();
            return Ok(new CreateCustomerResponse(displayDto));

        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateCustomerRequest request)
        {
            var operationResult = await _customerService.UpdateCustomerAsync(request.ToServiceRequest()).ConfigureAwait(false);
            if (!operationResult.Status)
            {
                _logger.LogError($"Error: Cannot update customer");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Cannot update customer");
            }

            var displayDto = operationResult.Data.Customer.ToDisplayDto();
            return Ok(new UpdateCustomerResponse(displayDto));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteCustomerRequest request)
        {
            var operationResult = await _customerService.DeleteCustomerAsync(request.ToServiceRequest()).ConfigureAwait(false);
            if (!operationResult.Status)
            {
                _logger.LogError($"Error: Cannot delete customer");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Cannot delete customer");
            }

            return Ok(new MessageResponse($"Successfully deleted customer: {operationResult.Data.CustomerId}"));
        }
    }
}
