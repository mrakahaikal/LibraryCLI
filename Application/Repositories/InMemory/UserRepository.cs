namespace Application.Repositories.InMemory;

using Application.Repositories.Interfaces;
using Domain.Entities;
using Infrastructure;

public class UserRepository : IUserRepository
{
    public List<User> GetAll() => DataStore.Users;
    public User? GetById(int id) => DataStore.Users.FirstOrDefault(u => u.Id == id);
    public void Add(User user) => DataStore.Users.Add(user);
    public bool Remove(int id)
    {
        var user = GetById(id);
        if (user is null) return false;
        DataStore.Users.Remove(user);
        return true;
    }

}
