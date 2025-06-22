using Domain.Entities;

namespace Infrastructure;

public static class DataStore
{
    public static List<Book> Books { get; } = new();
    public static List<User> Users { get; } = new();
    public static List<BorrowTransaction> Transactions { get; } = new();
}