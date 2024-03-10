using BookRenterData.Entities;
using BookRenterService.Models;

namespace BookRenterService.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(UserRequest userRequest);
        Task<User> ValidateCredentialsAsync(string username, string password);
    }
}