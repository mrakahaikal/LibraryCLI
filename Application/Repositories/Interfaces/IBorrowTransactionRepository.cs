namespace Application.Repositories.Interfaces;

using Domain.Entities;

public interface IBorrowTransactionRepository
{
    List<BorrowTransaction> GetAll();
    List<BorrowTransaction> GetActive();
    BorrowTransaction? GetById(int id);
    void Add(BorrowTransaction transaction);
    void Update(BorrowTransaction transaction);
}