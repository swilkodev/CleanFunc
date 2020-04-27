using System;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CleanFunc.Application.UnitTests
{
    public class ApplicationDbContextFactory
    {
        public static ApplicationDbContext Create()
        {
            var dateTimeService = new Mock<IDateTime>();
            dateTimeService.Setup(_ => _.Now).Returns(new DateTime(3000, 1, 1));
            
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options, dateTimeService.Object);
            ApplicationDbContextSeed.SeedSampleDataAsync(context, dateTimeService.Object);
            return context;
        }

        public static void Destroy(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
        
    }
}