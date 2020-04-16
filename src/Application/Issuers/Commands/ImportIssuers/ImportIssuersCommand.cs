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
using CleanFunc.Application.Common.Exceptions;

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
                var records = await this.reader.ReadAsync<IssuerRecord>(request.Data);

                // Check for duplicates
                var duplicates = records.GroupBy(record => record.Name)
                                        .Where(group => group.Count() > 1)
                                        .Select(group => group.Key);
                if(duplicates.Any())
                {
                    throw new DuplicateItemException("Name", duplicates);
                }

                // Map records to entity and add to database repository
                await repository.Add(
                                    records.Select(
                                            record => new Issuer{
                                                Id = Guid.NewGuid(),
                                                Name = record.Name,
                                                CreatedDate = record.CreatedDate,
                                            })
                );

                return records.LongCount();
            }
        }
    }
}