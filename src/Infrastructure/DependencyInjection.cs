using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Infrastructure.Context;
using CleanFunc.Infrastructure.Files;
using CleanFunc.Infrastructure.Persistence;
using CleanFunc.Infrastructure.ServiceBus;
using CleanFunc.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CleanFunc.Infrastructure
{
    [ExcludeFromCodeCoverageAttribute]
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {   
            services.AddTransient<IDateTime, DateTimeService>();        
            
            var serviceProvider = services.BuildServiceProvider();
            IConfiguration configuration = serviceProvider.GetService<IConfiguration>();

            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("AdminDb"));
            }
            else
            {
                string cosmosDbEndpoint = configuration.GetValue<string>("CosmosDBEndpoint");
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseCosmos(
                        cosmosDbEndpoint,
                        configuration.GetValue<string>("CosmosDBAccountKey"),
                        databaseName: "AdminDb")
                        );
            }
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            // if(cosmosDbEndpoint.Contains("localhost"))
            // {
            //     serviceProvider = services.BuildServiceProvider();
                
            //     AdminDbContextSeed.SeedSampleDataAsync(
            //         serviceProvider.GetService<AdminDbContext>(),
            //         serviceProvider.GetService<IDateTime>()
            //     );
            // }

            services.AddTransient<IAuditor, Auditor>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddCsvFile(Assembly.GetExecutingAssembly());

            // note: the below dependencies use a scope context (per call scope)
            services.AddScoped<IServiceBusConfiguration, ServiceBusConfiguration>();
            services.AddScoped<ICallContext, MutableCallContext>();
            services.AddScoped<IBusEndpointFactory,AzureServiceBusEndpointFactory>();
            services.AddScoped<IMessageEnricher,AzureServiceBusCausalityEnricher>();
            return services;
        }

        private static IServiceCollection AddCsvFile(this IServiceCollection services, Assembly assembly)
        {
            // Register all class maps in the assembly
            var csvClassMap = typeof(CsvHelper.Configuration.ClassMap);

            var classMapTypes = assembly
                .GetExportedTypes()
                .Where(t => t.IsSubclassOf(csvClassMap))
                .ToList();

            foreach (var classMap in classMapTypes)
            {
                services.AddTransient(csvClassMap, classMap);
            }
            
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
            services.AddTransient<ICsvFileReader, CsvFileReader>();
            
            return services;
        }
    }
}
