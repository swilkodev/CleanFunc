using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanFunc.Application.Common.Exceptions
{
    public class DuplicateItemException : Exception
    {
        public DuplicateItemException(string key, IEnumerable<string> duplicates)
            : base($"Duplicate items exist with the same {key} ({string.Join(',', duplicates.Select(_ => _))}).")
        {
        }

    }
}