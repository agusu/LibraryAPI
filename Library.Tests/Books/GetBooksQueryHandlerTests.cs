using Xunit;
using Moq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Application.Books.Queries;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Entities;
using System.Linq.Expressions;

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

    private static Mock<DbSet<T>> CreateDbSetMock<T>(IEnumerable<T> elements) where T : class
    {
        var elementsAsQueryable = elements.AsQueryable();
        var dbSetMock = new Mock<DbSet<T>>();

        dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elementsAsQueryable.Provider);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elementsAsQueryable.Expression);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elementsAsQueryable.ElementType);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => elementsAsQueryable.GetEnumerator());

        dbSetMock.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => elements.ToList().Add(s));

        return dbSetMock;
    }

    [Fact]
    public async Task Handle_GetBooks_ReturnsAllBooks()
    {
        // Arrange
        var books = new List<Book>
        {
            new() { Id = 1, Title = "Test Book", AuthorId = 1 }
        };

        var dbSetMock = CreateDbSetMock(books);
        _contextMock.Setup(c => c.Books).Returns(dbSetMock.Object);

        var expectedDto = new BookDto(1, "Test Book", "Description", DateTime.Now, 1, "Author Name");
        _mapperMock.Setup(m => m.Map<List<BookDto>>(It.IsAny<IEnumerable<Book>>()))
            .Returns(new List<BookDto> { expectedDto });

        var handler = new GetBooksQueryHandler(_contextMock.Object, _mapperMock.Object);

        // Act
        var result = await handler.Handle(new GetBooksQuery(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task Handle_GetBooksByAuthor_ReturnsFilteredBooks()
    {
        // Arrange
        var authorId = 1;
        var books = new List<Book>
        {
            new() { Id = 1, Title = "Test Book", AuthorId = authorId }
        };

        var dbSetMock = CreateDbSetMock(books);
        _contextMock.Setup(c => c.Books).Returns(dbSetMock.Object);

        var expectedDto = new BookDto(1, "Test Book", "Description", DateTime.Now, authorId, "Author Name");
        _mapperMock.Setup(m => m.Map<List<BookDto>>(It.IsAny<IEnumerable<Book>>()))
            .Returns(new List<BookDto> { expectedDto });

        var handler = new GetBooksByAuthorQueryHandler(_contextMock.Object, _mapperMock.Object);

        // Act
        var result = await handler.Handle(new GetBooksByAuthorQuery(authorId), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.All(result, book => Assert.Equal(authorId, book.AuthorId));
    }
}

// Clases auxiliares para testing asíncrono
public class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    public TestAsyncQueryProvider(IQueryProvider inner)
    {
        _inner = inner;
    }

    public IQueryable CreateQuery(Expression expression)
    {
        return new TestAsyncEnumerable<TEntity>(expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new TestAsyncEnumerable<TElement>(expression);
    }

    public object Execute(Expression expression)
    {
        return _inner.Execute(expression);
    }

    public TResult Execute<TResult>(Expression expression)
    {
        return _inner.Execute<TResult>(expression);
    }

    public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
    {
        return new TestAsyncEnumerable<TResult>(expression);
    }

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
        return Execute<TResult>(expression);
    }
}

public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(IEnumerable<T> enumerable)
        : base(enumerable)
    { }

    public TestAsyncEnumerable(Expression expression)
        : base(expression)
    { }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }
}

public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public TestAsyncEnumerator(IEnumerator<T> inner)
    {
        _inner = inner;
    }

    public T Current => _inner.Current;

    public ValueTask<bool> MoveNextAsync()
    {
        return new ValueTask<bool>(_inner.MoveNext());
    }

    public ValueTask DisposeAsync()
    {
        _inner.Dispose();
        return new ValueTask();
    }
}