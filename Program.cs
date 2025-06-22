using Domain.Entities;
using Infrastructure;
using Infrastructure.Seed;

Seeder.Seed();

bool isRunning = true;

while (isRunning)
{
    Console.Clear();
    Console.WriteLine("=== Sistem Peminjaman Buku ===");
    Console.WriteLine("1. Lihat Semua Buku");
    Console.WriteLine("2. Peminjaman Buku");
    Console.WriteLine("3. Pengembalian Buku");
    Console.WriteLine("4. Exit");
    Console.Write("Pilihan Anda");

    var input = Console.ReadLine();

    switch (input)
    {
        case "1":
            ShowAllBooks();
            break;
        case "2":
            BorrowBook();
            break;
        case "3":
            ReturnBook();
            break;
        case "4":
            isRunning = false;
            break;
        default:
            Console.WriteLine("Pilihan tidak valid, tekan ENTER untuk lanjut...");
            Console.ReadLine();
            break;

    }
}

static void ShowAllBooks()
{
    Console.Clear();
    Console.WriteLine("=== Daftar Buku ===");

    if (!DataStore.Books.Any())
    {
        Console.WriteLine("Belum ada buku dalam sistem.");
    }
    else
    {
        foreach (var book in DataStore.Books)
        {
            var status = book.IsBorrowed ? "❌ Dipinjam" : "✅ Tersedia";
            Console.WriteLine($"{book.Id}. {book.Title} - {book.Author} [{status}]");
        }

        Console.WriteLine("\n Tekan ENTER untuk kembali ke menu...");
        Console.ReadLine();
    }
}

static void BorrowBook()
{
    Console.Clear();
    Console.WriteLine("=== Peminjaman Buku ===");

    // Tampilkan daftar pengguna
    Console.WriteLine("\nDaftar Pengguna:");
    foreach (var u in DataStore.Users)
    {
        Console.WriteLine($"{u.Id}. {u.Name}");
    }

    Console.WriteLine("Masukkan ID User: ");
    if (!int.TryParse(Console.ReadLine(), out int userId))
    {
        Console.WriteLine("Input tidak valid!");
        Console.ReadLine();
        return;
    }

    var user = DataStore.Users.FirstOrDefault(u => u.Id == userId);
    if (user is null)
    {
        Console.WriteLine("Pengguna tidak ditemukan.");
        Console.ReadLine();
        return;
    }

    // Tampilkan buku yang tersedia
    var availableBooks = DataStore.Books.Where(b => !b.IsBorrowed).ToList();
    if (!availableBooks.Any())
    {
        Console.WriteLine("Semua buku sedang dipinjam.");
        Console.ReadLine();
        return;
    }

    Console.WriteLine("\nBuku Tersedia:");
    foreach (var book in availableBooks)
    {
        Console.WriteLine($"{book.Id}. {book.Title} karya {book.Author}");
    }

    Console.WriteLine("Masukkan ID Buku yang ingin dipinjam: ");
    if (!int.TryParse(Console.ReadLine(), out int bookId))
    {
        Console.WriteLine("Input tidak valid!");
        Console.ReadLine();
        return;
    }

    var bookToBorrow = availableBooks.FirstOrDefault(b => b.Id == bookId);
    if (bookToBorrow is null)
    {
        Console.WriteLine("Buku tidak tersedia atau ID salah.");
        Console.ReadLine();
        return;
    }

    // Proses peminjaman
    bookToBorrow.IsBorrowed = true;
    int transactionId = DataStore.Transactions.Count + 1;
    var transaction = new BorrowTransaction(transactionId, user, bookToBorrow);
    DataStore.Transactions.Add(transaction);

    Console.WriteLine($"\n ✅ Buku '{bookToBorrow.Title}' berhasil dipinjam oleh {user.Name}.");
    Console.WriteLine("Tekan ENTER untuk kembali ke menu...");
    Console.ReadLine();
}

static void ReturnBook()
{
    Console.Clear();
    Console.WriteLine("=== Pengembalian Buku ===");

    var activeTransactions = DataStore.Transactions
        .Where(t => t.ReturnedAt == null)
        .ToList();

    if (!activeTransactions.Any())
    {
        Console.WriteLine("Tidak ada transaksi aktif yang perlu dikembalikan.");
        Console.ReadLine();
        return;
    }

    Console.WriteLine("\nDaftar Buku yang Sedang Dipinjam:");
    foreach (var trx in activeTransactions)
    {
        Console.WriteLine($"{trx.Id}. \"{trx.BorrowedBook.Title}\" oleh {trx.Borrower.Name} (Dipinjam sejak: {trx.BorrowedAt})");
    }

    Console.Write("Masukkan ID transaksi yang ingin dikembalikan: ");
    if (!int.TryParse(Console.ReadLine(), out int transactionId))
    {
        Console.WriteLine("Input tidak valid!");
        Console.ReadLine();
        return;
    }

    var transaction = activeTransactions.FirstOrDefault(t => t.Id == transactionId);
    if (transaction is null)
    {
        Console.WriteLine("Transaksi tidak ditemukan atau sudah dikembalikan.");
        Console.ReadLine();
        return;
    }

    // Proses pengembalian
    transaction.ReturnedAt = DateTime.Now;
    transaction.BorrowedBook.IsBorrowed = false;

    Console.WriteLine($"\n✅ Buku \"{transaction.BorrowedBook.Title}\" berhasil dikembalikan oleh {transaction.Borrower.Name}.");
    Console.WriteLine("Tekan ENTER untuk kembali ke menu...");
    Console.ReadLine();
}
