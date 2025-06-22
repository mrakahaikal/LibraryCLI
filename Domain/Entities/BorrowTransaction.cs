using Domain.Interfaces;

namespace Domain.Entities;

public class BorrowTransaction : IIdentifiable
{
    public int Id { get; set; }
    public User Borrower { get; init; }
    public Book BorrowedBook { get; init; }
    public DateTime BorrowedAt { get; init; }
    public DateTime? ReturnedAt { get; set; }

    public BorrowTransaction(int id, User borrower, Book book)
    {
        Id = id;
        Borrower = borrower;
        BorrowedBook = book;
        BorrowedAt = DateTime.Now;
        ReturnedAt = null;
    }
}