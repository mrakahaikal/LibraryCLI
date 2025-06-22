using Domain.Interfaces;

namespace Domain.Entities;

public class Book : IIdentifiable
{
    public int Id { get; set; }
    public string Title { get; init; }
    public string Author { get; init; }
    public bool IsBorrowed { get; set; }

    public Book(int id, string title, string author)
    {
        Id = id;
        Title = title;
        Author = author;
        IsBorrowed = false;
    }
}