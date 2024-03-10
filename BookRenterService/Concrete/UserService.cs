using BookRenterData.Entities;
using BookRenterData.UnitOfWork.Interfaces;
using BookRenterService.Interfaces;
using BookRenterService.Models;
using System;
using System.Threading.Tasks;

namespace BookRenter.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<User> CreateUserAsync(UserRequest userRequest)
        {
            // Hash the password
            string passwordHash = HashPassword(userRequest.Password);

            // Create the user
            var user = new User
            {
                Username = userRequest.Username,
                PasswordHash = passwordHash                
            };

            // Add the user to the database
            await _unitOfWork.UserRepository.CreateUserAsync(user);
            await _unitOfWork.CompleteAsync();
            return user;
        }

        public async Task<User> ValidateCredentialsAsync(string username, string password)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                return null; // User not found or password incorrect
            }

            return user; // User credentials validated
        }

        private string HashPassword(string password)
        {
            // Use bcrypt for password hashing
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            // Verify password using bcrypt
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
