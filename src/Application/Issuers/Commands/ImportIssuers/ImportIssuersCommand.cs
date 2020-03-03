using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanFunc.Application.Common.Interfaces;
using CleanFunc.Domain.Entities;
using MediatR;
using System;
using CleanFunc.Application.Issuers.Models;
using Ardalis.GuardClauses;

namespace CleanFunc.Application.Issuers.Commands.ImportIssuers
{
    public class ImportIssuersCommand : IRequest<long>
    {
        public Stream Data {get;set;}

        public class ImportIssuersCommandHandler : IRequestHandler<ImportIssuersCommand, long>
        {
            private readonly ICsvFileReader reader;
            private readonly IIssuerRepository repository;
            private readonly IMapper mapper;

            public ImportIssuersCommandHandler(ICsvFileReader reader, IIssuerRepository repository, IMapper mapper)
            {
                Guard.Against.Null(reader, nameof(reader));
                Guard.Against.Null(repository, nameof(repository));
                Guard.Against.Null(mapper, nameof(mapper));

                this.reader = reader;
                this.repository = repository;
                this.mapper = mapper;
            }

            public async Task<long> Handle(ImportIssuersCommand request, CancellationToken cancellationToken)
            {
                var issuers=new List<Issuer>();

                foreach(var record in await this.reader.ReadAsync<IssuerRecord>(request.Data))
                {
                    issuers.Add(new Issuer
                    {
                        Id = Guid.NewGuid(),
                        Name = record.Name,
                        CreatedDate = record.CreatedDate,
                    });
                }

                await repository.Add(issuers);

                return issuers.LongCount();
            }
        }
    }
}