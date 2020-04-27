using System;
using System.Linq;
using System.Threading.Tasks;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Domain.Entities;
using CleanFunc.Infrastructure.Persistence;

namespace CleanFunc.Application.UnitTests
{
    public static class ApplicationDbContextSeed
    {
        public static void SeedSampleDataAsync(ApplicationDbContext context, IDateTime dateTime)
        {
            // Seed, if necessary
            if (!context.Issuers.Any())
            {
                context.Database.EnsureCreated();

                context.Issuers.Add(new Issuer { Id=new Guid("5f95d690-513a-497f-bba2-76bc286bf2af"), Name="SAW Beer Pty Ltd", CreatedDate=dateTime.Now});
                context.Issuers.Add(new Issuer { Id=new Guid("de891235-405e-4e72-912d-7bd51b4c92b7"), Name="Microsoft Corporation", CreatedDate=dateTime.Now.AddDays(- 1)});
                context.Issuers.Add(new Issuer { Id=new Guid("9e0dd491-cc18-4fe1-8f13-41a7e7270aef"), Name="Amazon.com, Inc.", CreatedDate=dateTime.Now.AddHours(-12)});

                context.SaveChanges();
            }
        }
    }
}