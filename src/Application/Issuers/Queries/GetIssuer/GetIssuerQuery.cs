using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using CleanFunc.Application.Common.Interfaces;
using MediatR;

namespace CleanFunc.Application.Issuers.Queries.GetIssuer
{

    public class GetIssuerQuery : IRequest<GetIssuerResponse>
    {
        public Guid Id {get;set;}

        public class GetIssuerQueryHandler : IRequestHandler<GetIssuerQuery, GetIssuerResponse>
        {
            private readonly IIssuerRepository _repository;
            private readonly IMapper _mapper;

            public GetIssuerQueryHandler(IIssuerRepository repository, IMapper mapper)
            {
                Guard.Against.Null(repository, nameof(repository));
                Guard.Against.Null(mapper, nameof(mapper));

                _repository = repository;
                _mapper = mapper;
            }

            public async Task<GetIssuerResponse> Handle(GetIssuerQuery request, CancellationToken cancellationToken)
            {
                var issuer = await _repository.GetById(request.Id);

                return new GetIssuerResponse
                {
                    Issuer = _mapper.Map<IssuerDto>(issuer)
                };
            }
        }
    }
}
