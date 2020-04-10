using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Application.Issuers.Models;
using CleanFunc.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanFunc.Application.Issuers.Queries.ExportIssuers
{
    public class ExportIssuersQuery : IRequest<ExportIssuersResponse>
    {
        // No members
        public Guid? Id {get;set;} 

        // Headers are injected by Azure Functions Runtime. Hmm!
        public IDictionary<string, string> Headers {get;set;} 

        public class ExportIssuersQueryHandler : IRequestHandler<ExportIssuersQuery, ExportIssuersResponse>
        {
            private readonly IIssuerRepository _issuerRepository;
            private readonly IMapper _mapper;
            private readonly ICsvFileBuilder _fileBuilder;
            private readonly ILogger<ExportIssuersQueryHandler> _logger;

            public ExportIssuersQueryHandler(IIssuerRepository issuerRepository, IMapper mapper, ICsvFileBuilder fileBuilder, ILogger<ExportIssuersQueryHandler> logger)
            {
                Guard.Against.Null(fileBuilder, nameof(fileBuilder));
                Guard.Against.Null(issuerRepository, nameof(issuerRepository));
                Guard.Against.Null(mapper, nameof(mapper));
                Guard.Against.Null(logger, nameof(logger));

                _issuerRepository = issuerRepository; 
                _mapper = mapper; 
                _fileBuilder = fileBuilder; 
                _logger = logger;
            }

            public async Task<ExportIssuersResponse> Handle(ExportIssuersQuery request, CancellationToken cancellationToken)
            {   
                IEnumerable<Issuer> issuers;
                if(request.Id == null)
                {
                    issuers = await _issuerRepository.GetAll();
                }
                else
                {
                    issuers = await _issuerRepository.GetWhere(x => x.Id == request.Id);
                }
                var records = issuers.AsQueryable()
                        .ProjectTo<IssuerRecord>(_mapper.ConfigurationProvider)
                        .ToList();
                
                var response = new ExportIssuersResponse()
                {
                    Content = await _fileBuilder.BuildFileAsync(records),
                    ContentType = "text/csv",
                    FileName = "Issuers.csv"
                };
                
                return response;    
            }
        }
    }
}