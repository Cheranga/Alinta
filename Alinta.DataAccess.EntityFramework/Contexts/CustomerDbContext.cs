using Alinta.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Alinta.DataAccess.EntityFramework.Contexts
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
    }
}