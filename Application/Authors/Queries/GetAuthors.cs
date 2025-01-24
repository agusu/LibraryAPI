using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Interfaces;
using Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.Authors.Queries
{
    public record GetAuthorsQuery : IRequest<List<AuthorDto>>;

    public class GetAuthorsQueryHandler : IRequestHandler<GetAuthorsQuery, List<AuthorDto>>
    {
        private readonly ILibraryContext _context;
        private readonly IMapper _mapper;

        public GetAuthorsQueryHandler(ILibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<List<AuthorDto>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
        {
            var authors = _context.Authors.ToList();
            return Task.FromResult(_mapper.Map<List<AuthorDto>>(authors));
        }
    }
}