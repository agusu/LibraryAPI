using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Application.DTOs;

namespace Application.Authors.Commands
{
    public record CreateAuthorCommand(CreateAuthorDto Author) : IRequest<AuthorDto>;

    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, AuthorDto>
    {
        private readonly ILibraryContext _context;
        private readonly IMapper _mapper;

        public CreateAuthorCommandHandler(ILibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AuthorDto> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = new Author
            {
                Name = request.Author.Name,
                DateOfBirth = request.Author.DateOfBirth,
                Nationality = request.Author.Nationality
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<AuthorDto>(author);
        }
    }
}