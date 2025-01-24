namespace Application.DTOs;

public record AuthorDto(
    int Id,
    string Name,
    DateTime DateOfBirth,
    string Nationality
);

public record CreateAuthorDto(
    string Name,
    DateTime DateOfBirth,
    string Nationality
);

public record UpdateAuthorDto(string Name);