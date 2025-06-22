using Domain.Entities;
using Infrastructure;
using Infrastructure.Seed;
using Application.Services;
using Application.Repositories.InMemory;
using Application.Helpers;

var bookRepo = new BookRepository();
var userRepo = new UserRepository();
var trxRepo = new BorrowTransactionRepository();

var borrowService = new BorrowService(bookRepo, userRepo, trxRepo);
var returnService = new ReturnService(bookRepo, trxRepo);
var bookService = new BookService(bookRepo);


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
    Console.Write("Pilihan Anda: ");

    var input = Console.ReadLine();

    switch (input)
    {
        case "1":
            ShowAllBooks(bookService);
            break;
        case "2":
            BorrowBook(borrowService);
            break;
        case "3":
            ReturnBook(returnService);
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

static void ShowAllBooks(BookService bookService)
{
    Console.Clear();
    Console.WriteLine("=== Daftar Buku ===");

    var books = bookService.GetAllBooks();
    if (!books.Any())
    {
        Console.WriteLine("Belum ada buku dalam sistem.");
    }
    else
    {
        ConsoleTableHelper.PrintHeader(("ID", 3), ("Judul", 40), ("Penulis", 20), ("Status", 10));

        foreach (var book in books)
        {
            var status = book.IsBorrowed ? "❌ Dipinjam" : "✅ Tersedia";
            ConsoleTableHelper.PrintRow(
                (book.Id.ToString(), 3),
                (book.Title, 40),
                (book.Author, 20),
                (status, 10)
            );
        }

        Console.WriteLine("\n Tekan ENTER untuk kembali ke menu...");
        Console.ReadLine();
    }
}

static void BorrowBook(BorrowService borrowService)
{
    Console.Clear();
    Console.WriteLine("=== Peminjaman Buku ===");

    // Tampilkan daftar pengguna
    Console.WriteLine("\nDaftar Pengguna:");
    ConsoleTableHelper.PrintHeader(("ID", 3), ("Nama", 20));
    foreach (var u in DataStore.Users)
    {
        ConsoleTableHelper.PrintRow(
            (u.Id.ToString(), 3),
            (u.Name, 20)
        );
    }

    Console.WriteLine("Masukkan ID User: ");
    if (!int.TryParse(Console.ReadLine(), out int userId))
    {
        Console.WriteLine("Input tidak valid!");
        Console.ReadLine();
        return;
    }

    // Tampilkan buku yang tersedia
    Console.WriteLine("\nDaftar Buku:");
    ConsoleTableHelper.PrintHeader(("ID", 3), ("Judul", 40), ("Penulis", 20));
    var availableBooks = DataStore.Books.Where(b => !b.IsBorrowed).ToList();
    if (!availableBooks.Any())
    {
        Console.WriteLine("Semua buku sedang dipinjam.");
        Console.WriteLine("Tekan ENTER untuk kembali ke menu...");
        Console.ReadLine();
        return;
    }

    foreach (var book in availableBooks)
    {
        ConsoleTableHelper.PrintRow(
            (book.Id.ToString(), 3),
            (book.Title, 40),
            (book.Author, 20)
        );
    }

    Console.WriteLine("Masukkan ID Buku yang ingin dipinjam: ");
    if (!int.TryParse(Console.ReadLine(), out int bookId))
    {
        Console.WriteLine("Input tidak valid!");
        Console.ReadLine();
        return;
    }


    // Proses peminjaman
    var success = borrowService.BorrowBook(userId, bookId, out var message);
    Console.WriteLine($"\n{message}");
    Console.WriteLine("Tekan ENTER untuk kembali ke menu...");
    Console.ReadLine();
}

static void ReturnBook(ReturnService returnService)
{
    Console.Clear();
    Console.WriteLine("=== Pengembalian Buku ===");

    // var activeTransactions = DataStore.Transactions
    //     .Where(t => t.ReturnedAt == null)
    //     .ToList();

    // if (!activeTransactions.Any())
    // {
    //     Console.WriteLine("Tidak ada transaksi aktif yang perlu dikembalikan.");
    //     Console.ReadLine();
    //     return;
    // }

    // Console.WriteLine("\nDaftar Buku yang Sedang Dipinjam:");
    // foreach (var trx in activeTransactions)
    // {
    //     Console.WriteLine($"{trx.Id}. \"{trx.BorrowedBook.Title}\" oleh {trx.Borrower.Name} (Dipinjam sejak: {trx.BorrowedAt})");
    // }

    Console.Write("Masukkan ID transaksi yang ingin dikembalikan: ");
    if (!int.TryParse(Console.ReadLine(), out int transactionId))
    {
        Console.WriteLine("Input tidak valid!");
        Console.ReadLine();
        return;
    }

    // var transaction = activeTransactions.FirstOrDefault(t => t.Id == transactionId);
    // if (transaction is null)
    // {
    //     Console.WriteLine("Transaksi tidak ditemukan atau sudah dikembalikan.");
    //     Console.ReadLine();
    //     return;
    // }

    // Proses pengembalian
    // transaction.ReturnedAt = DateTime.Now;
    // transaction.BorrowedBook.IsBorrowed = false;
    var success = returnService.ReturnBook(transactionId, out var message);
    Console.WriteLine($"\n{message}");
    // Console.WriteLine($"\n✅ Buku \"{transaction.BorrowedBook.Title}\" berhasil dikembalikan oleh {transaction.Borrower.Name}.");
    Console.WriteLine("Tekan ENTER untuk kembali ke menu...");
    Console.ReadLine();
}
