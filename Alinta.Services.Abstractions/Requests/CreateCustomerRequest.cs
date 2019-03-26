using System;
using Alinta.Core;
using Alinta.Services.Abstractions.Models;

namespace Alinta.Services.Abstractions.Requests
{
    public class CreateCustomerRequest : IValidate
    {
        public CustomerCreateModel Customer { get; }

        public CreateCustomerRequest(CustomerCreateModel customer)
        {
            Customer = customer;
        }

        public bool IsValid()
        {
            return Customer != null && !string.IsNullOrWhiteSpace(Customer.FirstName) &&
                   !string.IsNullOrWhiteSpace(Customer.LastName) &&
                   DateTime.Compare(DateTime.UtcNow, Customer.DateOfBirth.ToUniversalTime()) > 0;
        }
    }
}