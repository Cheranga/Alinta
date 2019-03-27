using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Alinta.Services.Abstractions.Models;
using Alinta.WebApi.Models;

namespace Alinta.WebApi.DTO.Responses
{
    public class SearchCustomerResponse
    {
        public SearchCustomerResponse(IEnumerable<DisplayCustomerDto> customers)
        {
            var list = customers?.ToList() ?? new List<DisplayCustomerDto>();
            Customers = new ReadOnlyCollection<DisplayCustomerDto>(list);
        }

        public ReadOnlyCollection<DisplayCustomerDto> Customers { get; }
    }
}