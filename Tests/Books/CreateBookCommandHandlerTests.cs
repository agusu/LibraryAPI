using Application.Books.Commands;
using Domain.Interfaces;
using Domain.Entities;
using Application.DTOs;

namespace Tests.Books;

public class CreateBookCommandHandlerTests
{
    private readonly Mock<ILibraryContext> _contextMock;
    private readonly Mock<IMapper> _mapperMock;

    public CreateBookCommandHandlerTests()
    {
        _contextMock = new Mock<ILibraryContext>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task Handle_ShouldCreateAndReturnBook()
    {
        var author = new Author { Id = 1, Name = "Test Author" };
        var createDto = new CreateBookDto(
            "Test Book",
            "Test Description",
            DateTime.Now,
            author.Id
        );
        var command = new CreateBookCommand(createDto);

        // Mock del autor existente
        _contextMock.Setup(x => x.Authors.FindAsync(author.Id))
            .ReturnsAsync(author);

        // Mock para verificar que se agregó el libro correcto
        Book? addedBook = null;
        _contextMock.Setup(x => x.Books.Add(It.IsAny<Book>()))
            .Callback<Book>(book => addedBook = book);

        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mapperMock.Setup(m => m.Map<BookDto>(It.IsAny<Book>()))
            .Returns((Book b) =>
            {
                Assert.NotNull(b.Author);
                return new BookDto(
                    b.Id,
                    b.Title,
                    b.Description,
                    b.PublicationDate,
                    b.AuthorId,
                    b.Author.Name
                );
            });

        var handler = new CreateBookCommandHandler(_contextMock.Object, _mapperMock.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(createDto.Title, result.Title);
        Assert.Equal(createDto.AuthorId, result.AuthorId);
        Assert.NotNull(addedBook);
        Assert.Equal(createDto.Title, addedBook.Title);
        Assert.Equal(author, addedBook.Author);
        _contextMock.Verify(x => x.Books.Add(It.IsAny<Book>()), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidAuthorId_ShouldThrowException()
    {
        // Arrange
        var createDto = new CreateBookDto(
            "Test Book",
            "Test Description",
            DateTime.Now,
            999 // Invalid AuthorId
        );
        var command = new CreateBookCommand(createDto);

        _contextMock.Setup(x => x.Authors.FindAsync(999))
            .ReturnsAsync(null as Author);

        var handler = new CreateBookCommandHandler(_contextMock.Object, _mapperMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(command, CancellationToken.None));
    }
}