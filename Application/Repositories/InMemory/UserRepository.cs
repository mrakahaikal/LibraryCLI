namespace Application.Repositories.InMemory;

using Application.Repositories.Interfaces;
using Domain.Entities;
using Infrastructure;

public class UserRepository : IUserRepository
{
    public List<User> GetAll() => DataStore.Users;
    public User? GetById(int id) => DataStore.Users.FirstOrDefault(u => u.Id == id);
}
