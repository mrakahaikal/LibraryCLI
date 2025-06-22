namespace Application.Repositories.Interfaces;

using Domain.Entities;

public interface IUserRepository
{
    List<User> GetAll();
    User? GetById(int id);
}