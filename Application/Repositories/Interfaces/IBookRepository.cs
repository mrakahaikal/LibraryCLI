namespace Application.Repositories.Interfaces;

using Domain.Entities;

public interface IBookRepository
{
    List<Book> GetAll();
    List<Book> GetAvailable();
    Book? GetById(int Id);
    void Update(Book book);
    void Add(Book book);
    bool Remove(int id);
}