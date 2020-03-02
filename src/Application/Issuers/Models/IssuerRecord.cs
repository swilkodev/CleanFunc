using System;
using CleanFunc.Application.Common.Mappings;
using CleanFunc.Domain.Entities;

namespace CleanFunc.Application.Issuers.Models
{
    // CSV record
    public class IssuerRecord : IMapFrom<Issuer>
    {
        public string Name {get;set;}

        public DateTime CreatedDate {get;set;}
    }
}