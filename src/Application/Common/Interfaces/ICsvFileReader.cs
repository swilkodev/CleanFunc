using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CleanFunc.Application.Common.Interfaces
{
    public interface ICsvFileReader
    {
         Task<IEnumerable<TRecord>> ReadAsync<TRecord>(Stream stream);
    }
}