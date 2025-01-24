namespace Application.DTOs;

public record BookDto(
    int Id,
    string Title,
    string Description,
    DateTime PublicationDate,
    int AuthorId,
    string AuthorName
);

public record CreateBookDto(
    string Title,
    string Description,
    DateTime PublicationDate,
    int AuthorId
);

public record UpdateBookDto(
    string Title,
    string Description,
    DateTime PublicationDate,
    int AuthorId
);