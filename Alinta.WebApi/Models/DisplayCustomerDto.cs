namespace Alinta.WebApi.Models
{
    public class DisplayCustomerDto
    {
        public DisplayCustomerDto(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; }
    }
}