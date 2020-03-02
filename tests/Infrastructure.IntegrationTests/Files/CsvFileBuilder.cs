using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CleanFunc.Infrastructure.Files;
using Xunit;

namespace Infrastructure.IntegrationTests.Files
{
    public class CsvFileBuilderTests
    {
        [Fact]
        public async Task CsvFileBuilder_ShouldResultInCorrectFormat()
        {
            var record = new FooRecord
            {
                SomeField = "TEST",
                AnotherField = "TEST2"
            };

            var records = new List<FooRecord>();
            records.Add(record);
            var fileBuilder = new CsvFileBuilder();
            
            // act
            var bytes = await fileBuilder.BuildFileAsync<FooRecord>(records);

            // assert
            var str = Encoding.UTF8.GetString(bytes);
            Assert.Contains("TEST,TEST2", str);
        }
    }

    public class FooRecord
    {
        public string SomeField { get; set; }
        public string AnotherField { get; set; }
    }
}