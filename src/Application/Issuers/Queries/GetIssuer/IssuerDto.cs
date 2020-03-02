using System;
using AutoMapper;
using CleanFunc.Application.Common.Mappings;
using CleanFunc.Domain.Entities;

namespace CleanFunc.Application.Issuers.Queries.GetIssuer
{
    //Map from Domain Object to Dto(Contract)
    public class IssuerDto : IMapFrom<Issuer>
    {
        public string Name {get;set;}

        public DateTime CreatedDate {get;set;}
    }
}