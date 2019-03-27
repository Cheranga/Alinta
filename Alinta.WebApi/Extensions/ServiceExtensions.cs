using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alinta.Services.Abstractions.Models;
using Alinta.Services.Abstractions.Requests;

namespace Alinta.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static CreateCustomerRequest ToServiceRequest(this DTO.Requests.CreateCustomerRequest request)
        {
            if (request == null)
            {
                return null;
            }

            return new CreateCustomerRequest(new CustomerCreateModel(request.FirstName, request.LastName, request.DateOfBirth));
        }

        public static SearchCustomersRequest ToServiceRequest(this DTO.Requests.SearchCustomerRequest request)
        {
            if (request == null)
            {
                return null;
            }

            return new SearchCustomersRequest(request.Name);
        }

        public static DeleteCustomerRequest ToServiceRequest(this DTO.Requests.DeleteCustomerRequest request)
        {
            if (request == null)
            {
                return null;
            }

            return new DeleteCustomerRequest(request.CustomerId);
        }
    }
}
