using MediatR;
using Domain.Interfaces;

namespace Application.Authors.Commands;

public record DeleteAuthorCommand(int Id) : IRequest<bool>;

public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, bool>
{
    private readonly ILibraryContext _context;

    public DeleteAuthorCommandHandler(ILibraryContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await _context.Authors.FindAsync(request.Id);
        if (author == null) return false;

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}