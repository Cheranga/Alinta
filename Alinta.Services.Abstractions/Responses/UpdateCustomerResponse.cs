using Alinta.Services.Abstractions.Models;

namespace Alinta.Services.Abstractions.Responses
{
    public class UpdateCustomerResponse
    {
        public CustomerDisplayModel Customer { get; }

        public UpdateCustomerResponse(CustomerDisplayModel customer)
        {
            Customer = customer;
        }
    }
}