using System.Collections.Generic;
using System.Threading.Tasks;
using Alinta.Core;
using Alinta.DataAccess.Models;

namespace Alinta.DataAccess.Abstractions.Repositories
{
    public interface ICustomerRepository
    {
        Task<OperationResult<List<Customer>>> GetCustomersByNameAsync(string search);
        Task<OperationResult> CreateCustomerAsync(Customer customer);
    }
}