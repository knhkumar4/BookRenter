using BookRenterData.Entities;
using BookRenterData.Repositories.Base.Interfaces;

namespace BookRenterRepository.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
         Task<User> CreateUserAsync(User user);
        Task<User> GetUserByUsernameAsync(string username);
    }
}