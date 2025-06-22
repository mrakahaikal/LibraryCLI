namespace Application.Services;

using Application.Repositories.Interfaces;
using Domain.Entities;

public class BookService
{
    private readonly IBookRepository _bookRepo;

    public BookService(IBookRepository bookRepo)
    {
        _bookRepo = bookRepo;
    }

    public List<Book> GetAllBooks() => _bookRepo.GetAll();
    public List<Book> GetAvailableBooks() => _bookRepo.GetAvailable();
}