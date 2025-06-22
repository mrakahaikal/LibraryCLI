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
    Console.WriteLine("4. Tambah Buku");
    Console.WriteLine("5. Hapus Buku");
    Console.WriteLine("6. Tambah User");
    Console.WriteLine("7. Hapus User");
    Console.WriteLine("8. Exit");
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
            AddNewBook(bookRepo);
            break;
        case "5":
            DeleteBook(bookRepo);
            break;
        case "6":
            AddNewUser(userRepo);
            break;
        case "7":
            DeleteUser(userRepo);
            break;
        case "8":
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
        ConsoleTableHelper.PrintHeader(
            new ColumnCell("ID", 3),
            new ColumnCell("Judul", 40),
            new ColumnCell("Penulis", 20), 
            new ColumnCell("Status", 10)
        );

        foreach (var book in books)
        {
            var status = book.IsBorrowed ? "❌ Dipinjam" : "✅ Tersedia";
            ConsoleTableHelper.PrintRow(
                new ColumnCell(book.Id.ToString(), 3),
                new ColumnCell(book.Title, 40),
                new ColumnCell(book.Author, 20),
                new ColumnCell(status, 10)
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
    ConsoleTableHelper.PrintHeader(
        new ColumnCell("ID", 3),
        new ColumnCell("Nama", 20)
    );
    foreach (var u in DataStore.Users)
    {
        ConsoleTableHelper.PrintRow(
            new ColumnCell(u.Id.ToString(), 3),
            new ColumnCell(u.Name, 20)
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
    ConsoleTableHelper.PrintHeader(
        new ColumnCell("ID", 3),
        new ColumnCell("Judul", 40),
        new ColumnCell("Penulis", 20)
    );
    var availableBooks = DataStore.Books.Where(b => !b.IsBorrowed).ToList();
    if (!availableBooks.Any())
    {
        ConsoleTableHelper.PrintRow(new ColumnCell("Semua buku sedang dipinjam.", 62,  Align.Center) );
        Console.WriteLine("Tekan ENTER untuk kembali ke menu...");
        Console.ReadLine();
        return;
    }

    foreach (var book in availableBooks)
    {
        ConsoleTableHelper.PrintRow(
            new ColumnCell(book.Id.ToString(), 3),
            new ColumnCell(book.Title, 40),
            new ColumnCell(book.Author, 20)
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

    var activeTransactions = DataStore.Transactions
        .Where(t => t.ReturnedAt == null)
        .ToList();

    Console.WriteLine("\nDaftar Buku yang Sedang Dipinjam:");
    ConsoleTableHelper.PrintHeader(
        new ColumnCell("ID", 3),
        new ColumnCell("Judul", 40),
        new ColumnCell("Peminjam", 20),
        new ColumnCell("Dipinjam Sejak", 20)
    );
    if (!activeTransactions.Any())
    {
        ConsoleTableHelper.PrintHeader(new ColumnCell("Tidak ada transaksi aktif yang perlu dikembalikan.", 92, Align.Center));
        Console.WriteLine("Tekan ENTER untuk kembali ke menu...");
        Console.ReadLine();
        return;
    }

    foreach (var trx in activeTransactions)
    {
        ConsoleTableHelper.PrintRow(
            new ColumnCell(trx.Id.ToString(), 3),
            new ColumnCell(trx.BorrowedBook.Title, 40),
            new ColumnCell(trx.Borrower.Name, 20),
            new ColumnCell(trx.BorrowedAt.ToString(), 20)
        );
    }

    Console.Write("Masukkan ID transaksi yang ingin dikembalikan: ");
    if (!int.TryParse(Console.ReadLine(), out int transactionId))
    {
        Console.WriteLine("Input tidak valid!");
        Console.ReadLine();
        return;
    }

    var success = returnService.ReturnBook(transactionId, out var message);
    Console.WriteLine($"\n{message}");
    Console.WriteLine("Tekan ENTER untuk kembali ke menu...");
    Console.ReadLine();
}

static void AddNewBook(BookRepository repo)
{
    Console.Clear();
    Console.WriteLine("=== Tambah Buku Baru ===");

    Console.Write("Judul Buku\t: ");
    var title = Console.ReadLine() ?? "";

    Console.Write("Penulis\t\t: ");
    var author = Console.ReadLine() ?? "";

    int nextId = repo.GetAll().Count + 1;
    var newBook = new Book(nextId, title, author);
    repo.Add(newBook);

    Console.WriteLine($"\n✅ Buku \"{title}\" berhasil ditambahkan.");
    Console.WriteLine("Tekan ENTER untuk kembali...");
    Console.ReadLine();
}

static void DeleteBook(BookRepository repo)
{
    Console.Clear();
    Console.WriteLine("=== Hapus Buku ===");

    Console.Write("Masukkan ID Buku: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID tidak valid.");
    }
    else if (repo.Remove(id))
    {
        Console.WriteLine("✅ Buku berhasil dihapus.");
    }
    else {

        Console.WriteLine("❌ Buku tidak ditemukan.");
    }

    Console.WriteLine("Tekan ENTER untuk kembali...");
    Console.ReadLine();
}

static void AddNewUser(UserRepository repo)
{
    Console.Clear();
    Console.WriteLine("=== Tambah Pengguna Baru ===");

    Console.Write("Nama\t: ");
    var name = Console.ReadLine() ?? "";


    int nextId = repo.GetAll().Count + 1;
    var newUser = new User(nextId, name);
    repo.Add(newUser);

    Console.WriteLine($"\n✅ Pengguna baru bernama \"{name}\" berhasil ditambahkan.");
    Console.WriteLine("Tekan ENTER untuk kembali...");
    Console.ReadLine();
}

static void DeleteUser(UserRepository repo)
{
    Console.Clear();
    Console.WriteLine("=== Hapus Pengguna ===");

    Console.Write("Masukkan ID Pengguna: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID tidak valid.");
    }
    else if (repo.Remove(id))
    {
        Console.WriteLine("✅ Pengguna berhasil dihapus.");
    }
    else {

        Console.WriteLine("❌ Pengguna tidak ditemukan.");
    }

    Console.WriteLine("Tekan ENTER untuk kembali...");
    Console.ReadLine();
}