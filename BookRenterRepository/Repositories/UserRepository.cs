using BookRenterData.Context;
using BookRenterData.Entities;
using BookRenterData.Repositories.Base;
using BookRenterData.Repositories.Base.Interfaces;
using BookRenterRepository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookRenterData.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly BookRenterContext _dbContext;

        public UserRepository(BookRenterContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<User> CreateUserAsync(User user)
        {
            // Check if the username is already taken
            if (await _dbContext.Users.AnyAsync(u => u.Username == user.Username))
            {
                throw new InvalidOperationException("Username is already taken.");            }

            // Add the user to the database
            await _dbContext.Users.AddAsync(user);         

            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }



    }
}
