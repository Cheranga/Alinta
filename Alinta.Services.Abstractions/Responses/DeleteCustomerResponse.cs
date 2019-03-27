namespace Alinta.Services.Abstractions.Responses
{
    public class DeleteCustomerResponse
    {
        public string CustomerId { get; }

        public DeleteCustomerResponse(string customerId)
        {
            CustomerId = customerId;
        }
    }
}