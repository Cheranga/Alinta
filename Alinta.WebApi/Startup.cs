﻿using Alinta.Services;
using Alinta.Services.Abstractions.Responses;
using Alinta.WebApi.Presenters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Alinta.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            RegisterDependencies(services);
            services.UseAlintaServices();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //
            // Setup swagger
            //
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Alinta API", Version = "v1" });
            });
        }

        private void RegisterDependencies(IServiceCollection services)
        {
            services.AddSingleton<IOperationResultPresenter<CreateCustomerResponse>, CreateCustomerResponsePresenter>();
            services.AddSingleton<IOperationResultPresenter<UpdateCustomerResponse>, UpdateCustomerResponsePresenter>();
            services.AddSingleton<IOperationResultPresenter<DeleteCustomerResponse>, DeleteCustomerResponsePresenter>();
            services.AddSingleton<IOperationResultPresenter<SearchCustomersResponse>, SearchCustomersResponsePresenter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            //
            // Use swagger
            //
            //var option = new RewriteOptions();
            //option.AddRedirect("^$", "swagger");
            //app.UseRewriter(option);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Alinta API V1");
            });
            app.UseMvc();
        }
    }
}
