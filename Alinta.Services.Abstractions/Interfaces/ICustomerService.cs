using System.Threading.Tasks;
using Alinta.Core;
using Alinta.Services.Abstractions.Requests;
using Alinta.Services.Abstractions.Responses;

namespace Alinta.Services.Abstractions.Interfaces
{
    public interface ICustomerService
    {
        Task<OperationResult<CreateCustomerResponse>> CreateCustomerAsync(CreateCustomerRequest request);
        Task<OperationResult<UpdateCustomerResponse>> UpdateCustomerAsync(UpdateCustomerRequest request);
        Task<OperationResult<DeleteCustomerResponse>> DeleteCustomerAsync(DeleteCustomerRequest request);
        Task<OperationResult<SearchCustomersResponse>> SearchCustomersAsync(SearchCustomersRequest request);
    }
}
