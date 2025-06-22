# 📚 LibraryCLI — A Console App for Book Borrowing Management

LibraryCLI adalah aplikasi berbasis .NET yang berjalan di terminal dan dirancang untuk mengelola peminjaman buku secara sederhana namun terstruktur. Cocok digunakan sebagai fondasi belajar arsitektur modular dan pemisahan logika bisnis menggunakan pendekatan service-repository.

## ✨ Fitur Utama

- Menampilkan daftar buku beserta status ketersediaannya
- Proses peminjaman buku oleh pengguna
- Pengembalian buku dengan validasi transaksi
- Penyimpanan data secara in-memory
- Struktur kode modular: `Entities`, `Repositories`, `Services`, dan `CLI`
- Dukungan tampilan tabel yang rapi di konsol

## 🏗️ Arsitektur

Proyek ini mengikuti pendekatan layering yang jelas:

Program.cs (CLI UI)
↓
Services (BorrowService, ReturnService, BookService)
↓
Repositories (InMemoryBookRepository, UserRepository, etc)
↓
Entities + DataStore (In-Memory Simulation)

Semua business logic dipisah dari antarmuka CLI sehingga mudah diubah ke REST API, GUI, maupun microservice di masa depan.

## 🚀 Cara Menjalankan

1. Pastikan .NET SDK 6 ke atas sudah terinstal
2. Clone repo ini
3. Jalankan dari terminal:

```bash
dotnet run
```

## 📂 Struktur Folder

/Domain → Entitas (Book, User, BorrowTransaction)
/Application → Repositories & Services
/Infrastructure → DataStore & Utils (TablePrinter)
Program.cs → CLI utama dengan top-level statements

## 📦 Roadmap Berikutnya

[ ] Export transaksi ke file

[ ] Validasi tanggal dan peminjam ganda

[ ] Refactor ke Clean Architecture (ASP.NET Core)

[ ] Integrasi EF Core

## 🧑‍💻 Dibuat oleh

Raka Haikal — Developer yang metodis dan progresif, dengan semangat eksplorasi teknologi backend berbasis .NET.

> "Code is clean if it makes it easier for someone else to enhance it." — Uncle Bob
