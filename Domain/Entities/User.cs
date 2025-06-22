using Domain.Interfaces;

namespace Domain.Entities;

public class User : IIdentifiable
{
    public int Id { get; set; }
    public string Name { get; init; }

    public User(int id, string name)
    {
        Id = id;
        Name = name;
    }
}