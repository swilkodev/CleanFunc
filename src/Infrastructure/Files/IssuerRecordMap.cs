using System.Globalization;
using CleanFunc.Application.Issuers.Models;

namespace CleanFunc.Infrastructure.Files
{
    internal class IssuerRecordMap : CsvHelper.Configuration.ClassMap<IssuerRecord>
    {
        public IssuerRecordMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            //Map(m => m..Done).ConvertUsing(c => c.Done ? "Yes" : "No");
        }
    }
}