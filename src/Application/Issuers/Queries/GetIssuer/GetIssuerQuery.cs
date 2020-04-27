using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanFunc.Application.Issuers.Queries.GetIssuer
{

    public class GetIssuerQuery : IRequest<GetIssuerResponse>
    {
        public Guid Id {get;set;}

        public class GetIssuerQueryHandler : IRequestHandler<GetIssuerQuery, GetIssuerResponse>
        {
            private readonly IApplicationDbContext _dbContext;
            private readonly IMapper _mapper;

            public GetIssuerQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
            {
                Guard.Against.Null(dbContext, nameof(dbContext));
                Guard.Against.Null(mapper, nameof(mapper));

                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<GetIssuerResponse> Handle(GetIssuerQuery request, CancellationToken cancellationToken)
            {
                var issuer = await _dbContext.Issuers.FirstOrDefaultAsync(_ => _.Id == request.Id);

                return new GetIssuerResponse
                {
                    Issuer = _mapper.Map<IssuerDto>(issuer)
                };
            }
        }
    }
}
