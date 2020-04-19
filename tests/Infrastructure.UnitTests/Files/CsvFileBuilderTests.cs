using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CleanFunc.Infrastructure.Files;
using Xunit;
using CsvHelper.Configuration;
using FluentAssertions;

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
                AnotherField = "TEST2",
                Done=true
            };
            var record2 = new FooRecord
            {
                SomeField = "TEST3",
                AnotherField = "TEST4",
                Done=false
            };
            var records = new List<FooRecord>();
            records.Add(record);
            records.Add(record2);

            var fileMaps = new List<ClassMap>();
            fileMaps.Add(new FooRecordMap());
            var sut = new CsvFileBuilder(fileMaps);
            
            // act
            var bytes = await sut.BuildFileAsync<FooRecord>(records);

            // assert
            var sr = new StringReader(Encoding.UTF8.GetString(bytes));
            var line1 = sr.ReadLine();
            var line2 = sr.ReadLine();
            var line3 = sr.ReadLine();

            line1.Should().Be("SomeField,AnotherField,Done");
            line2.Should().Be("TEST,TEST2,Yes");
            line3.Should().Be("TEST3,TEST4,No");
        }
    }
}