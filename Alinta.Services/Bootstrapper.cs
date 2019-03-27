using Alinta.DataAccess.EntityFramework;
using Alinta.Services.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Alinta.Services
{
    public static class Bootstrapper
    {
        public static void UseAlintaServices(this IServiceCollection services)
        {
            //
            // Register dependencies in here
            //
            services.AddScoped<ICustomerService, CustomerService>();
            
            services.UseAlintaDataAccess();
        }
    }
}
