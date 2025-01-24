using MediatR;
using AutoMapper;
using Domain.Interfaces;
using Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.Books.Queries;

public record GetBookByIdQuery(int Id) : IRequest<BookDto?>;

public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDto?>
{
    private readonly ILibraryContext _context;
    private readonly IMapper _mapper;

    public GetBookByIdQueryHandler(ILibraryContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BookDto?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _context.Books
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

        return book != null ? _mapper.Map<BookDto>(book) : null;
    }
}