using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Alinta.Services.Abstractions.Models;

namespace Alinta.Services.Abstractions.Responses
{
    public class SearchCustomersResponse
    {
        public SearchCustomersResponse(IEnumerable<CustomerDisplayModel> customers)
        {
            var list = customers?.ToList() ?? new List<CustomerDisplayModel>();
            Customers = new ReadOnlyCollection<CustomerDisplayModel>(list);
        }

        public ReadOnlyCollection<CustomerDisplayModel> Customers { get; }
    }
}