using MediatR;
using AutoMapper;
using Domain.Interfaces;
using Domain.Entities;
using Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.Authors.Queries
{
    public record GetAuthorByIdQuery(int Id) : IRequest<AuthorDto?>;

    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, AuthorDto?>
    {
        private readonly ILibraryContext _context;
        private readonly IMapper _mapper;

        public GetAuthorByIdQueryHandler(ILibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AuthorDto?> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            var author = await _context.Authors.FindAsync(request.Id);
            return author != null ? _mapper.Map<AuthorDto>(author) : null;
        }
    }
}