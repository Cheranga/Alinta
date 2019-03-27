using System;
using System.Collections.Generic;
using System.Linq;
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
                    .Where(x=> EF.Functions.Like(x.FirstName,search) || EF.Functions.Like(x.LastName, search))
                    .ToListAsync().ConfigureAwait(false);

                result = OperationResult<List<Customer>>.Success(filteredCustomers?? new List<Customer>());
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Error when looking for customer by name: {search}");

                result = OperationResult<List<Customer>>.Failure($"Error when looking for customer by name: {search}");
            }

            return result;
        }

        public async Task<OperationResult<Customer>> CreateCustomerAsync(Customer customer)
        {
            if (!customer.Validate())
            {
                _logger.LogError($"Error: Invalid customer");
                return OperationResult<Customer>.Failure("Error: Invalid customer");
            }

            OperationResult<Customer> result;
            try
            {
                await _context.Customers.AddAsync(customer).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);

                result = OperationResult<Customer>.Success(customer);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error: Cannot create customer");
                result = OperationResult<Customer>.Failure("Cannot create customer");
            }

            return result;
        }

        public async Task<OperationResult> DeleteCustomerAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return OperationResult.Failure($"Error: Invalid id {id}");
            }

            OperationResult result;
            try
            {
                var customer = await _context.Customers.FindAsync(id).ConfigureAwait(false);
                if (customer == null)
                {
                    result = OperationResult.Failure($"Error: There is no customer by id {id}");
                }
                else
                {
                    _context.Customers.Remove(customer);
                    await _context.SaveChangesAsync();

                    result = OperationResult.Success();
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Error: Cannot delete customer by id: {id}");
                result = OperationResult.Failure($"Error: Cannot delete customer by id: {id}");
            }

            return result;
        }

        public async Task<OperationResult<Customer>> UpdateCustomerAsync(Customer customer)
        {
            if (!customer.Validate())
            {
                return OperationResult<Customer>.Failure("Error: Invalid customer cannot update");
            }

            OperationResult<Customer> result;
            try
            {
                var existingCustomer = await _context.Customers.SingleOrDefaultAsync(x => x.Id == customer.Id).ConfigureAwait(false);
                if (existingCustomer == null)
                {
                    result = OperationResult<Customer>.Failure($"Error: Cannot update customer, customer does not exist");
                }
                else
                {
                    existingCustomer.FirstName = customer.FirstName;
                    existingCustomer.LastName = customer.LastName;
                    existingCustomer.DateOfBirth = customer.DateOfBirth;

                    await _context.SaveChangesAsync();

                    result = OperationResult<Customer>.Success(existingCustomer);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error: Invalid customer cannot update");
                result = OperationResult<Customer>.Failure("Error: Invalid customer cannot update");
            }

            return result;
        }
    }
}
