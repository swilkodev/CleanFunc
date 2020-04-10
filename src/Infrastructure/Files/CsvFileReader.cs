using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CleanFunc.Application.Common.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;

namespace CleanFunc.Infrastructure.Files
{
    public class CsvFileReader : ICsvFileReader
    {
        private IEnumerable<ClassMap> _classMaps;

        public CsvFileReader(IEnumerable<ClassMap> classMaps)
        {
            _classMaps = classMaps;
        }

        // NOTE: We are not returning AsyncEnumerable here due to problems with
        //  1. deferred execution
        //  2. resources which are disposed after the method finishes execution
        // 
        // Deferred calls to this method will result in errors due to objects being disposed
        public async Task<IEnumerable<TRecord>> ReadAsync<TRecord>(Stream stream)
        {
            using(var streamReader = new StreamReader(stream))
            using(var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                // register all class maps
                foreach(CsvHelper.Configuration.ClassMap c in _classMaps)
                {
                    csvReader.Configuration.RegisterClassMap(c);   
                    // csvReader.Configuration.BadDataFound = (ctx) => 
                    // {
                    //     throw new ValidationException(ctx);
                    // }; 
                }

                var records = new List<TRecord>();
                await foreach(var record in csvReader.GetRecordsAsync<TRecord>())
                {
                    records.Add(record);
                }

                return records;
            }
        }
    }
}