using System;
using Alinta.Core;

namespace Alinta.DataAccess.Models
{
    public class Customer : IValidate
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public bool IsValid() => Id > 0 &&
                                 !string.IsNullOrWhiteSpace(FirstName) &&
                                 !string.IsNullOrWhiteSpace(LastName);
    }
}