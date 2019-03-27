using System;
using System.Collections.Generic;
using System.Text;
using Alinta.DataAccess.EntityFramework;
using Alinta.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Alinta.Facade
{
    public static class Bootstrapper
    {
        public static void UseAlintaFacade(this IServiceCollection services)
        {
            //
            // Register dependencies in here
            //
            services.UseAlintaServices();
            services.UseAlintaDataAccess();
        }
    }
}
