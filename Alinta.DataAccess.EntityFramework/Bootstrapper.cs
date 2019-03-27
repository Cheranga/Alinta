using Alinta.DataAccess.Abstractions.Repositories;
using Alinta.DataAccess.EntityFramework.Contexts;
using Alinta.DataAccess.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Alinta.DataAccess.EntityFramework
{
    public static class Bootstrapper
    {
        public static void UseAlintaDataAccess(this IServiceCollection services)
        {
            //
            // Register dependencies in here
            //
            services.AddDbContext<CustomerDbContext>(options => options.UseInMemoryDatabase(databaseName: "AlintaDb"));
            services.AddScoped<ICustomerRepository, CustomerRepository>();
        }
    }
}
