using Infrastructure;
using Domain.Entities;

namespace Infrastructure.Seed;

public static class Seeder
{
    public static void Seed()
    {
        DataStore.Books.AddRange(new List<Book>
        {
            new Book(1, "Clean Code", "Robert C. Martin"),
            new Book(2, "The Pragmatic Programmer", "Andy Hunt"),
            new Book(3, "Introduction to Algorithms", "CLRS"),
        });

        DataStore.Users.AddRange(new List<User>
        {
            new User(1, "Raka"),
            new User(2, "Yushi"),
            new User(3, "John"),
        });


    }
}