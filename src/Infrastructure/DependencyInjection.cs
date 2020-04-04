using System.Diagnostics.CodeAnalysis;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Infrastructure.Context;
using CleanFunc.Infrastructure.Files;
using CleanFunc.Infrastructure.Persistence;
using CleanFunc.Infrastructure.ServiceBus;
using CleanFunc.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CleanFunc.Infrastructure
{
    [ExcludeFromCodeCoverageAttribute]
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {           
            
            services.AddTransient<IIssuerRepository, IssuerRepository>();
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
            services.AddTransient<ICsvFileReader, CsvFileReader>();
            services.AddTransient<IAuditor, Auditor>();
            services.AddTransient<IEmailService, EmailService>();
            // note: the below dependencies use a scope context (per call scope)
            services.AddScoped<ICallContext, MutableCallContext>();
            services.AddScoped<IBusFactory,ServiceBusFactory>();
            services.AddScoped<IMessageEnricher,AzureFunctionServiceBusCausalityEnricher>();
            return services;
        }
    }
}
