using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanFunc.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
         Task<byte[]> BuildFileAsync<TRecord>(IEnumerable<TRecord> records);
    }
}