using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ILibraryContext
    {
        DbSet<Author> Authors { get; }
        DbSet<Book> Books { get; }
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}