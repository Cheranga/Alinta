using Alinta.Core;

namespace Alinta.WebApi.DTO.Requests
{
    public class SearchCustomerRequest : IValidate
    {
        public string Name { get; set; }
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
    }
}
