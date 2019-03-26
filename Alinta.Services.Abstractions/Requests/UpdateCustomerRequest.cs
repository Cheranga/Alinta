using System;
using Alinta.Core;
using Alinta.Services.Abstractions.Models;

namespace Alinta.Services.Abstractions.Requests
{
    public class UpdateCustomerRequest : IValidate
    {
        public CustomerUpdateModel Customer { get; }

        public UpdateCustomerRequest(CustomerUpdateModel customer)
        {
            Customer = customer;
        }

        public bool IsValid()
        {
            return Customer != null && !string.IsNullOrWhiteSpace(Customer.Id) &&
                   !string.IsNullOrWhiteSpace(Customer.FirstName) &&
                   !string.IsNullOrWhiteSpace(Customer.LastName) &&
                   DateTime.Compare(DateTime.UtcNow, Customer.DateOfBirth.ToUniversalTime()) > 0;
        }
    }
}