using System;
using Alinta.Core;

namespace Alinta.DataAccess.Models
{
    public class Customer : IValidate
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public bool IsValid() => !string.IsNullOrWhiteSpace(Id) &&
                                 !string.IsNullOrWhiteSpace(FirstName) &&
                                 !string.IsNullOrWhiteSpace(LastName);
    }
}