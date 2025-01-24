using MediatR;
using AutoMapper;
using Domain.Interfaces;
using Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.Books.Queries;

public record GetBooksByAuthorQuery(int AuthorId) : IRequest<List<BookDto>>;

public class GetBooksByAuthorQueryHandler : IRequestHandler<GetBooksByAuthorQuery, List<BookDto>>
{
    private readonly ILibraryContext _context;
    private readonly IMapper _mapper;

    public GetBooksByAuthorQueryHandler(ILibraryContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<BookDto>> Handle(GetBooksByAuthorQuery request, CancellationToken cancellationToken)
    {
        var books = _context.Books
            .Include(b => b.Author)
            .Where(b => b.AuthorId == request.AuthorId)
            .ToList();

        return Task.FromResult(_mapper.Map<List<BookDto>>(books));
    }
}