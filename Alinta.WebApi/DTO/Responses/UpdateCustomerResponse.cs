using Alinta.WebApi.Models;

namespace Alinta.WebApi.DTO.Responses
{
    public class UpdateCustomerResponse
    {
        public DisplayCustomerDto Customer { get; }

        public UpdateCustomerResponse(DisplayCustomerDto customer)
        {
            Customer = customer;
        }
    }
}