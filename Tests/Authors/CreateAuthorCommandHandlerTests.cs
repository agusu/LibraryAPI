using Application.Authors.Commands;
using Domain.Interfaces;
using Domain.Entities;
using Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Tests.Authors;

public class CreateAuthorCommandHandlerTests
{
    private readonly Mock<ILibraryContext> _contextMock;
    private readonly Mock<IMapper> _mapperMock;

    public CreateAuthorCommandHandlerTests()
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
    public async Task Handle_ShouldCreateAndReturnAuthor()
    {
        var authors = new List<Author>();
        var dbSetMock = CreateDbSetMock(authors);
        _contextMock.Setup(x => x.Authors).Returns(dbSetMock.Object);

        var createDto = new CreateAuthorDto("Test Author", DateTime.Now, "Test Nationality");
        var expectedAuthor = new Author
        {
            Id = 1,
            Name = createDto.Name,
            DateOfBirth = createDto.DateOfBirth,
            Nationality = createDto.Nationality
        };
        var expectedDto = new AuthorDto(1, createDto.Name, createDto.DateOfBirth, createDto.Nationality);

        _mapperMock.Setup(m => m.Map<AuthorDto>(It.IsAny<Author>()))
            .Returns(expectedDto);

        var handler = new CreateAuthorCommandHandler(_contextMock.Object, _mapperMock.Object);

        var result = await handler.Handle(new CreateAuthorCommand(createDto), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(expectedDto.Name, result.Name);
    }
}