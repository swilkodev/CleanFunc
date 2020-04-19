using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CleanFunc.Infrastructure.Files;
using Xunit;
using CsvHelper.Configuration;
using FluentAssertions;
using System.Globalization;

namespace Infrastructure.IntegrationTests.Files
{
    public class CsvFileReaderTests
    {
        [Fact]
        public async Task CsvFileReader_ShouldReadIn2Records()
        {
            // arrange
            var fileMaps = new List<ClassMap>();
            fileMaps.Add(new FooRecordMap());
            var sut = new CsvFileReader(fileMaps);

            var sb = new StringBuilder();
            sb.AppendLine("SomeField,AnotherField,Done");
            sb.AppendLine("TEST,TEST2,Yes");
            sb.AppendLine("TEST3,TEST4,No");
            var ms = new MemoryStream( Encoding. UTF8. GetBytes( sb.ToString() ) );

            // act
            var records = await sut.ReadAsync<FooRecord>(ms);

            // assert
            records.Count().Should().Be(2);
            var recordOne = records.First();
            recordOne.SomeField.Should().Be("TEST");
            recordOne.AnotherField.Should().Be("TEST2");
            recordOne.Done.Should().Be(true);
            var recordTwo = records.Skip(1).First();
            recordTwo.SomeField.Should().Be("TEST3");
            recordTwo.AnotherField.Should().Be("TEST4");
            recordTwo.Done.Should().Be(false);
        }
    }
}