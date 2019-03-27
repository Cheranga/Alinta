using System;
using Alinta.Core;

namespace Alinta.WebApi.DTO.Requests
{
    public class CreateCustomerRequest : IValidate
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        //
        // To simulate that these can be "Application / API" specific validations
        //
        public bool IsValid() => !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName);
    }
}