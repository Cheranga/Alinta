using Alinta.Core;

namespace Alinta.Services.Abstractions.Requests
{
    public class DeleteCustomerRequest : IValidate
    {
        public string CustomerId { get; }

        public DeleteCustomerRequest(string customerId)
        {
            CustomerId = customerId;
        }

        public bool IsValid() => !string.IsNullOrWhiteSpace(CustomerId);
    }
}