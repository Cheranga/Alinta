using System;
using System.Linq;
using System.Threading.Tasks;
using Alinta.Core;
using Alinta.DataAccess.Abstractions.Repositories;
using Alinta.Services.Abstractions.Interfaces;
using Alinta.Services.Abstractions.Requests;
using Alinta.Services.Abstractions.Responses;
using Alinta.Services.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Alinta.Services
{
    public class CustomerService: ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ICustomerRepository customerRepository, ILogger<CustomerService> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public async Task<OperationResult<CreateCustomerResponse>> CreateCustomerAsync(CreateCustomerRequest request)
        {
            var isValid = request.Validate();
            if (!isValid)
            {
                _logger.LogError($"Error. Invalid request: {JsonConvert.SerializeObject(request)}");
                return OperationResult<CreateCustomerResponse>.Failure("Invalid request");
            }

            var operationResult = await _customerRepository.CreateCustomerAsync(request.Customer.ToDataAccess()).ConfigureAwait(false);
            if (!operationResult.Status)
            {
                _logger.LogError($"Error: {operationResult.Message}");
                return OperationResult<CreateCustomerResponse>.Failure("Cannot create customer, error occured");
            }

            var displayCustomer = operationResult.Data.ToDisplay();

            return OperationResult<CreateCustomerResponse>.Success(new CreateCustomerResponse(displayCustomer));
        }

        public async Task<OperationResult<UpdateCustomerResponse>> UpdateCustomerAsync(UpdateCustomerRequest request)
        {
            var isValid = request.Validate();
            if (!isValid)
            {
                _logger.LogError($"Error. Invalid request: {JsonConvert.SerializeObject(request)}");
                return OperationResult<UpdateCustomerResponse>.Failure("Invalid request");
            }

            var operationResult = await _customerRepository.UpdateCustomerAsync(request.Customer.ToDataAccess()).ConfigureAwait(false);
            if (!operationResult.Status)
            {
                _logger.LogError($"Error: {operationResult.Message}");
                return OperationResult<UpdateCustomerResponse>.Failure("Cannot create customer, error occured");
            }

            var displayCustomer = operationResult.Data.ToDisplay();

            return OperationResult<UpdateCustomerResponse>.Success(new UpdateCustomerResponse(displayCustomer));
        }

        public async Task<OperationResult<DeleteCustomerResponse>> DeleteCustomerAsync(DeleteCustomerRequest request)
        {
            var isValid = request.Validate();
            if (!isValid)
            {
                _logger.LogError($"Error. Invalid request: {JsonConvert.SerializeObject(request)}");
                return OperationResult<DeleteCustomerResponse>.Failure("Invalid request");
            }

            var operationResult = await _customerRepository.DeleteCustomerAsync(request.CustomerId).ConfigureAwait(false);
            if (!operationResult.Status)
            {
                _logger.LogError($"Error: {operationResult.Message}");
                return OperationResult<DeleteCustomerResponse>.Failure("Cannot delete customer, error occured");
            }

            return OperationResult<DeleteCustomerResponse>.Success(new DeleteCustomerResponse(request.CustomerId));
        }

        public async Task<OperationResult<SearchCustomersResponse>> SearchCustomersAsync(SearchCustomersRequest request)
        {
            var isValid = request.Validate();
            if (!isValid)
            {
                _logger.LogError($"Error. Invalid request: {JsonConvert.SerializeObject(request)}");
                return OperationResult<SearchCustomersResponse>.Failure("Invalid request");
            }

            var operationResult = await _customerRepository.GetCustomersByNameAsync(request.Name).ConfigureAwait(false);
            if (!operationResult.Status)
            {
                _logger.LogError($"Error: {operationResult.Message}");
                return OperationResult<SearchCustomersResponse>.Failure("Cannot search for customers, error occured");
            }

            var displayCustomers = operationResult.Data.Select(x => x.ToDisplay());

            return OperationResult<SearchCustomersResponse>.Success(new SearchCustomersResponse(displayCustomers));
        }
    }
}
