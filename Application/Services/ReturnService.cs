namespace Application.Services;

using Application.Repositories.Interfaces;
using Domain.Entities;

public class ReturnService
{
    private readonly IBookRepository _bookRepo;
    private readonly IBorrowTransactionRepository _transactionRepo;

    public ReturnService(
        IBookRepository bookRepo,
        IBorrowTransactionRepository transactionRepo)
    {
        _bookRepo = bookRepo;
        _transactionRepo = transactionRepo;
    }

    public bool ReturnBook(int transactionId, out string message)
    {
        var trx = _transactionRepo.GetById(transactionId);
        if (trx is null || trx.ReturnedAt is not null)
        {
            message = "❌ Transaksi tidak ditemukan atau buku sudah dikembalikan.";
            return false;
        }

        trx.ReturnedAt = DateTime.Now;
        trx.BorrowedBook.IsBorrowed = false;

        _bookRepo.Update(trx.BorrowedBook);
        _transactionRepo.Update(trx);

        message = $"✅ Buku \"{trx.BorrowedBook.Title}\" berhasil dikembalikan.";
        return true;
    }
}