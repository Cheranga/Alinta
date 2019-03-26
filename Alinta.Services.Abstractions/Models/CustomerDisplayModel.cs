namespace Alinta.Services.Abstractions.Models
{
    public class CustomerDisplayModel
    {
        public CustomerDisplayModel(string id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public string Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string FullName => $"{FirstName} {LastName}";
    }
}