namespace Application.Repositories.InMemory;

using Application.Repositories.Interfaces;
using Domain.Entities;
using Infrastructure;

public class BookRepository : IBookRepository
{
    public List<Book> GetAll() => DataStore.Books;
    public List<Book> GetAvailable() => DataStore.Books.Where(b => !b.IsBorrowed).ToList();
    public Book? GetById(int id) => DataStore.Books.FirstOrDefault(b => b.Id == id);
    public void Update(Book book)
    {
        // Empty
    }
}