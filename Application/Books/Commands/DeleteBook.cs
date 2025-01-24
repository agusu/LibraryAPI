using MediatR;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Books.Commands;

public record DeleteBookCommand(int Id) : IRequest<bool>;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, bool>
{
    private readonly ILibraryContext _context;

    public DeleteBookCommandHandler(ILibraryContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _context.Books.FindAsync(request.Id);
        if (book == null) return false;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}