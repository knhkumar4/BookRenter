using BookRenterData.Entities;

namespace BookRenterService.Concrete
{
    public interface IUserClaimService
    {
        Task<User> GetUserFromClaimAsync();
    }
}