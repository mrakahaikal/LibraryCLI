namespace Application.Repositories.InMemory;

using Application.Repositories.Interfaces;
using Domain.Entities;
using Infrastructure;

public class BorrowTransactionRepository : IBorrowTransactionRepository
{
    public List<BorrowTransaction> GetAll() => DataStore.Transactions;
    public List<BorrowTransaction> GetActive() => DataStore.Transactions.Where(t => t.ReturnedAt == null).ToList();
    public BorrowTransaction? GetById(int id) => DataStore.Transactions.FirstOrDefault(t => t.Id == id);
    public void Add(BorrowTransaction transaction) => DataStore.Transactions.Add(transaction);
    public void Update(BorrowTransaction transaction)
    {
        // No-op: DataStore menyimpan referensi, jadi objek sudah terupdate
    }
}