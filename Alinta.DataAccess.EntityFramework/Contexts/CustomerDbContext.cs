using Alinta.DataAccess.EntityFramework.EntityConfigurations;
using Alinta.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Alinta.DataAccess.EntityFramework.Contexts
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        }

        public DbSet<Customer> Customers { get; set; }
    }
}