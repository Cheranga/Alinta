namespace Alinta.Services.Abstractions.Requests
{
    public class DeleteCustomerRequest
    {
        public int CustomerId { get; }

        public DeleteCustomerRequest(int customerId)
        {
            CustomerId = customerId;
        }
    }
}