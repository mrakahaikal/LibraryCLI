namespace Application.Services;

using Application.Repositories.Interfaces;
using Domain.Entities;

public class BorrowService
{
    private readonly IBookRepository _bookRepo;
    private readonly IUserRepository _userRepo;
    private readonly IBorrowTransactionRepository _transactionRepo;

    public BorrowService(
        IBookRepository bookRepo,
        IUserRepository userRepo,
        IBorrowTransactionRepository transactionRepo)
    {
        _bookRepo = bookRepo;
        _userRepo = userRepo;
        _transactionRepo = transactionRepo;
    }

    public bool BorrowBook(int userId, int bookId, out string message)
    {
        var user = _userRepo.GetById(userId);
        if (user is null)
        {
            message = "❌ Pengguna tidak ditemukan.";
            return false;
        }

        var book = _bookRepo.GetById(bookId);
        if (book is null)
        {
            message = "❌ Buku tidak tersedia untuk dipinjam.";
            return false;
        }

        // Proses peminjaman
        book.IsBorrowed = true;
        _bookRepo.Update(book);

        int transactionId = _transactionRepo.GetAll().Count + 1;
        var transaction = new BorrowTransaction(transactionId, user, book);
        _transactionRepo.Add(transaction);

        message = $"✅ Buku {book.Title} berhasil dipinjam oleh {user.Name}.";
        return true;
    }
}