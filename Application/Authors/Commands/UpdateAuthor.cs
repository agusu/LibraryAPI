using MediatR;
using AutoMapper;
using Domain.Interfaces;
using Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.Authors.Commands;

public record UpdateAuthorCommand(int Id, UpdateAuthorDto Author) : IRequest<AuthorDto?>;

public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, AuthorDto?>
{
    private readonly ILibraryContext _context;
    private readonly IMapper _mapper;

    public UpdateAuthorCommandHandler(ILibraryContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AuthorDto?> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await _context.Authors.FindAsync(request.Id);
        if (author == null) return null;

        author.Name = request.Author.Name;
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AuthorDto>(author);
    }
}