namespace Domain.Entities;
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime PublicationDate { get; set; }
    public int AuthorId { get; set; }
    public Author? Author { get; set; }
}
