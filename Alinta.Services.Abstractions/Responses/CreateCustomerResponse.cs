using Alinta.Services.Abstractions.Models;

namespace Alinta.Services.Abstractions.Responses
{
    public class CreateCustomerResponse
    {
        public CustomerDisplayModel Customer { get; }

        public CreateCustomerResponse(CustomerDisplayModel customer)
        {
            Customer = customer;
        }
    }
}