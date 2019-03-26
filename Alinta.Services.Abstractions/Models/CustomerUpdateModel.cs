using System;

namespace Alinta.Services.Abstractions.Models
{
    public class CustomerUpdateModel
    {
        public string Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public DateTime DateOfBirth { get; }

        public CustomerUpdateModel(string id, string firstName, string lastName, DateTime dateOfBirth)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
        }
    }
}