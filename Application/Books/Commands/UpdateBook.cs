using MediatR;
using AutoMapper;
using Domain.Interfaces;
using Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.Books.Commands;

public record UpdateBookCommand(int Id, UpdateBookDto Book) : IRequest<BookDto?>;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookDto?>
{
    private readonly ILibraryContext _context;
    private readonly IMapper _mapper;

    public UpdateBookCommandHandler(ILibraryContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BookDto?> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _context.Books
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

        if (book == null) return null;

        book.Title = request.Book.Title;
        book.Description = request.Book.Description;
        book.PublicationDate = request.Book.PublicationDate;
        book.AuthorId = request.Book.AuthorId;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<BookDto>(book);
    }
}