using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Alinta.Services.Abstractions.Interfaces;
using Alinta.Services.Abstractions.Responses;
using Alinta.WebApi.DTO.Requests;
using Alinta.WebApi.Extensions;
using Alinta.WebApi.Presenters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CreateCustomerRequest = Alinta.WebApi.DTO.Requests.CreateCustomerRequest;
using CreateCustomerResponse = Alinta.WebApi.DTO.Responses.CreateCustomerResponse;

namespace Alinta.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IOperationResultPresenter<Services.Abstractions.Responses.CreateCustomerResponse> _createCustomerResponsePresenter;
        private readonly IOperationResultPresenter<SearchCustomersResponse> _searchCustomerResponsePresenter;
        private readonly IOperationResultPresenter<UpdateCustomerResponse> _updateCustomerResponsePresenter;
        private readonly IOperationResultPresenter<DeleteCustomerResponse> _deleteCustomerResponsePresenter;
        private readonly ILogger<CustomersController> _logger;


        public CustomersController(ICustomerService customerService, IOperationResultPresenter<Services.Abstractions.Responses.CreateCustomerResponse> createCustomerResponsePresenter,
            IOperationResultPresenter<SearchCustomersResponse> searchCustomerResponsePresenter,
            IOperationResultPresenter<UpdateCustomerResponse> updateCustomerResponsePresenter,
            IOperationResultPresenter<DeleteCustomerResponse> deleteCustomerResponsePresenter,
            ILogger<CustomersController> logger)
        {
            _customerService = customerService;
            _createCustomerResponsePresenter = createCustomerResponsePresenter;
            _searchCustomerResponsePresenter = searchCustomerResponsePresenter;
            _updateCustomerResponsePresenter = updateCustomerResponsePresenter;
            _deleteCustomerResponsePresenter = deleteCustomerResponsePresenter;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]SearchCustomerRequest request)
        {
            var operationResult = await _customerService.SearchCustomersAsync(request.ToServiceRequest()).ConfigureAwait(false);
            if (!operationResult.Status)
            {
                _logger.LogError($"Error: Cannot search for customers");
            }

            var actionResult = _searchCustomerResponsePresenter.Handle(operationResult);
            return actionResult;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateCustomerRequest request)
        {
            var operationResult = await _customerService.CreateCustomerAsync(request.ToServiceRequest()).ConfigureAwait(false);
            if (!operationResult.Status)
            {
                _logger.LogError($"Error: Cannot create customer");
            }

            var actionResult = _createCustomerResponsePresenter.Handle(operationResult);

            return actionResult;

        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateCustomerRequest request)
        {
            var operationResult = await _customerService.UpdateCustomerAsync(request.ToServiceRequest()).ConfigureAwait(false);
            if (!operationResult.Status)
            {
                _logger.LogError($"Error: Cannot update customer");
            }

            var actionResult = _updateCustomerResponsePresenter.Handle(operationResult);
            return actionResult;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteCustomerRequest request)
        {
            var operationResult = await _customerService.DeleteCustomerAsync(request.ToServiceRequest()).ConfigureAwait(false);
            if (!operationResult.Status)
            {
                _logger.LogError($"Error: Cannot delete customer");
            }

            var actionResult = _deleteCustomerResponsePresenter.Handle(operationResult);
            return actionResult;
        }
    }
}
