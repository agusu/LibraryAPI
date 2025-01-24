using Microsoft.EntityFrameworkCore;
using Application.Books.Queries;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Entities;

namespace Library.Tests.Books;

public class GetBooksQueryHandlerTests
{
    private readonly Mock<ILibraryContext> _contextMock;
    private readonly Mock<IMapper> _mapperMock;

    public GetBooksQueryHandlerTests()
    {
        _contextMock = new Mock<ILibraryContext>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task Handle_GetBooks_ReturnsAllBooks()
    {
        var books = new List<Book>
        {
            new() { Id = 1, Title = "Test Book", AuthorId = 1 }
        }.AsQueryable();

        var dbSetMock = CreateDbSetMock(books);

        _contextMock.Setup(c => c.Books).Returns(dbSetMock.Object);

        var expectedDto = new BookDto(1, "Test Book", "Description", DateTime.Now, 1, "Author Name");
        _mapperMock.Setup(m => m.Map<List<BookDto>>(It.IsAny<List<Book>>()))
            .Returns(new List<BookDto> { expectedDto });

        var handler = new GetBooksQueryHandler(_contextMock.Object, _mapperMock.Object);

        var result = await handler.Handle(new GetBooksQuery(), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task Handle_GetBooksByAuthor_ReturnsFilteredBooks()
    {
        var authorId = 1;
        var books = new List<Book>
        {
            new() { Id = 1, Title = "Test Book", AuthorId = authorId },
            new() { Id = 2, Title = "Test Book By Different Author", AuthorId = 2 }
        };

        var dbSetMock = CreateDbSetMock(books);
        _contextMock.Setup(c => c.Books).Returns(dbSetMock.Object);

        var expectedDto = new BookDto(1, "Test Book", "Description", DateTime.Now, authorId, "Author Name");
        _mapperMock.Setup(m => m.Map<List<BookDto>>(It.IsAny<IEnumerable<Book>>()))
            .Returns(new List<BookDto> { expectedDto });

        var handler = new GetBooksByAuthorQueryHandler(_contextMock.Object, _mapperMock.Object);

        var result = await handler.Handle(new GetBooksByAuthorQuery(authorId), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.All(result, book => Assert.Equal(authorId, book.AuthorId));
    }

    private static Mock<DbSet<T>> CreateDbSetMock<T>(IEnumerable<T> elements) where T : class
    {
        var elementsAsQueryable = elements.AsQueryable();
        var dbSetMock = new Mock<DbSet<T>>();

        dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elementsAsQueryable.Provider);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elementsAsQueryable.Expression);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elementsAsQueryable.ElementType);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => elementsAsQueryable.GetEnumerator());

        dbSetMock.As<IAsyncEnumerable<T>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<T>(elements.GetEnumerator()));

        return dbSetMock;
    }

    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public T Current => _inner.Current;

        public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext());

        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return new ValueTask();
        }
    }
}