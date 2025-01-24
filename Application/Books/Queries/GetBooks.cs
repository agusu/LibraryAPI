using MediatR;
using AutoMapper;
using Domain.Interfaces;
using Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.Books.Queries;

public record GetBooksQuery() : IRequest<List<BookDto>>;

public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, List<BookDto>>
{
    private readonly ILibraryContext _context;
    private readonly IMapper _mapper;

    public GetBooksQueryHandler(ILibraryContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var books = _context.Books.Include(b => b.Author).ToList();
        return Task.FromResult(_mapper.Map<List<BookDto>>(books));
    }
}