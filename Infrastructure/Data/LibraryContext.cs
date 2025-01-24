using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Pomelo.EntityFrameworkCore.MySql;

namespace Infrastructure.Data
{
    public class LibraryContext : DbContext, ILibraryContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        public DbSet<Book> Books => Set<Book>();
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId);
        }
    }
}