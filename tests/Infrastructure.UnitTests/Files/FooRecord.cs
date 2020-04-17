using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Infrastructure.IntegrationTests.Files
{
    public class FooRecord
    {
        public string SomeField { get; set; }
        public string AnotherField { get; set; }
        public bool Done {get;set;}
    }

    public class FooRecordMap : CsvHelper.Configuration.ClassMap<FooRecord>
    {
        public FooRecordMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.Done).TypeConverter(new YesNoBooleanConverter());
        }
    }

    public class YesNoBooleanConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if(text.Equals("yes", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            return false;
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return (bool)value ? "Yes" : "No";
        }
    }

}