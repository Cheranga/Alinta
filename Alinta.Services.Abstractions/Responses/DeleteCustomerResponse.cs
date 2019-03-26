using Alinta.Services.Abstractions.Models;

namespace Alinta.Services.Abstractions.Responses
{
    public class DeleteCustomerResponse
    {
        public CustomerDisplayModel Customer { get; }

        public DeleteCustomerResponse(CustomerDisplayModel customer)
        {
            Customer = customer;
        }
    }
}