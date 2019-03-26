using Alinta.DataAccess.Models;
using Alinta.Services.Abstractions.Models;

namespace Alinta.Services.Extensions
{
    public static class ModelExtensions
    {
        public static Customer ToDataAccess(this CustomerCreateModel model)
        {
            return new Customer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth
            };
        }

        public static CustomerDisplayModel ToDisplay(this Customer customer)
        {
            return new CustomerDisplayModel(customer.Id, customer.FirstName, customer.LastName);
        }

        public static Customer ToDataAccess(this CustomerUpdateModel model)
        {
            return new Customer
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth
            };
        }
    }
}