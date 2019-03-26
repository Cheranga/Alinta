using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alinta.Core;
using Alinta.DataAccess.Abstractions.Repositories;
using Alinta.DataAccess.EntityFramework.Contexts;
using Alinta.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Alinta.DataAccess.EntityFramework.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(CustomerDbContext context, ILogger<CustomerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<OperationResult<List<Customer>>> GetCustomersByNameAsync(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                _logger.LogError("Error: Null or empty search term");
                return OperationResult<List<Customer>>.Failure("Please specify a name to to search");
            }

            OperationResult<List<Customer>> result;
            try
            {
                var filteredCustomers = await _context.Customers.AsNoTracking()
                    .Where(x => x.FirstName == search || x.LastName == search)
                    .ToListAsync().ConfigureAwait(false);

                result = OperationResult<List<Customer>>.Success(filteredCustomers);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Error when looking for customer by name: {search}");

                result = OperationResult<List<Customer>>.Failure($"Error when looking for customer by name: {search}");
            }

            return result;
        }

        public async Task<OperationResult> CreateCustomerAsync(Customer customer)
        {
            if (customer == null)
            {
                return OperationResult.Failure("Error: Customer is null");
            }

            OperationResult result;
            try
            {
                await _context.Customers.AddAsync(customer).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                

                result = OperationResult.Success();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error: Cannot create customer");
                result = OperationResult.Failure("Cannot create customer");
            }

            return result;
        }
    }
}
