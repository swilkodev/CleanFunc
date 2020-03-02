using CleanFunc.Application;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Infrastructure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(CleanFunc.FunctionApp.Startup))]

namespace CleanFunc.FunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;

            services.AddScoped<IMessageEnricher,AzureFunctionServiceBusCausalityEnricher>();
            
            services.AddApplication();

            // TODO The Azure Function should not have any dependency on Infrastructure other than DI
            // Determine a better technique
            services.AddInfrastructure();
        }
    }
}