using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Application.Issuers.Queries.ExportIssuers;
using CsvHelper;
using CsvHelper.Configuration;

namespace CleanFunc.Infrastructure.Files
{
    public class CsvFileBuilder : ICsvFileBuilder
    {
        private List<ClassMap> classMaps=new List<ClassMap>();

        public CsvFileBuilder()
        {
            classMaps.Add(new IssuerRecordMap());
        }

        public async Task<byte[]> BuildFileAsync<TRecord>(System.Collections.Generic.IEnumerable<TRecord> records)
        {
            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream))
            {
                using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

                // register all class maps
                foreach(CsvHelper.Configuration.ClassMap c in classMaps)
                {
                    csvWriter.Configuration.RegisterClassMap(c);    
                }
                
                await csvWriter.WriteRecordsAsync(records);
            }

            return memoryStream.ToArray();
        }
    }
}