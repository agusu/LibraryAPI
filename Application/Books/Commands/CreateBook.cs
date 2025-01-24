using MediatR;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Application.DTOs;

namespace Application.Books.Commands;

public record CreateBookCommand(CreateBookDto Book) : IRequest<BookDto>;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookDto>
{
    private readonly ILibraryContext _context;
    private readonly IMapper _mapper;

    public CreateBookCommandHandler(ILibraryContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BookDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var author = await _context.Authors.FindAsync(request.Book.AuthorId);
        if (author == null)
            throw new InvalidOperationException($"Author with ID {request.Book.AuthorId} not found");

        var book = new Book
        {
            Title = request.Book.Title,
            Description = request.Book.Description,
            PublicationDate = request.Book.PublicationDate,
            AuthorId = request.Book.AuthorId,
            Author = author
        };

        _context.Books.Add(book);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<BookDto>(book);
    }
}