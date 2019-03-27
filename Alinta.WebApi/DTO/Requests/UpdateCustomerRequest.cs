using System;

namespace Alinta.WebApi.DTO.Requests
{
    public class UpdateCustomerRequest
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}