using BookRenterService.Models;

namespace BookRenterService.Interfaces
{
    public interface ICheckoutService
    {
        Task<CheckoutResponse> CheckoutBooksAsync();
    }
}