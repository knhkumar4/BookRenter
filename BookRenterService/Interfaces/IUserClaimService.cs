using BookRenterData.Entities;

namespace BookRenterService.Interfaces
{
    public interface IUserClaimService
    {
        Task<User> GetUserFromClaimAsync();
    }
}