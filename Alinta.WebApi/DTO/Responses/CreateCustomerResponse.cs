using Alinta.WebApi.Models;

namespace Alinta.WebApi.DTO.Responses
{
    public class CreateCustomerResponse
    {
        public DisplayCustomerDto Customer { get; }

        public CreateCustomerResponse(DisplayCustomerDto customer)
        {
            Customer = customer;
        }
    }
}