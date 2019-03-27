using Alinta.Services.Abstractions.Models;
using Alinta.WebApi.Models;

namespace Alinta.WebApi.Extensions
{
    public static class ModelExtensions
    {
        public static DisplayCustomerDto ToDisplayDto(this CustomerDisplayModel customer)
        {
            return new DisplayCustomerDto(customer.Id, customer.FullName);
        }
    }
}