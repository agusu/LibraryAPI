using Microsoft.EntityFrameworkCore;
using Application.Authors.Queries;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Entities;

namespace Tests.Authors;

public class GetAuthorsQueryHandlerTests
{
    private readonly Mock<ILibraryContext> _contextMock;
    private readonly Mock<IMapper> _mapperMock;

    public GetAuthorsQueryHandlerTests()
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

        return dbSetMock;
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfAuthors()
    {
        var authors = new List<Author>
        {
            new() { Id = 1, Name = "Test Author", DateOfBirth = DateTime.Now, Nationality = "Test" }
        };

        var dbSetMock = CreateDbSetMock(authors);
        _contextMock.Setup(x => x.Authors).Returns(dbSetMock.Object);

        var expectedDto = new AuthorDto(1, "Test Author", DateTime.Now, "Test");
        _mapperMock.Setup(m => m.Map<List<AuthorDto>>(It.IsAny<IEnumerable<Author>>()))
            .Returns(new List<AuthorDto> { expectedDto });

        var handler = new GetAuthorsQueryHandler(_contextMock.Object, _mapperMock.Object);

        var result = await handler.Handle(new GetAuthorsQuery(), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Test Author", result[0].Name);
    }
}