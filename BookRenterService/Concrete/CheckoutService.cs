using System;
using System.Threading.Tasks;
using BookRenter.Services.Interfaces;
using BookRenterData.Entities;
using BookRenterData.UnitOfWork.Interfaces;
using BookRenterService.Interfaces;

namespace BookRenter.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserClaimService _userClaimService;

        public CheckoutService(IUnitOfWork unitOfWork, IUserClaimService userClaimService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userClaimService = userClaimService ?? throw new ArgumentNullException(nameof(userClaimService));
        }

      

        public async Task<string> CheckoutBooksAsync()
        {
            var user = await _userClaimService.GetUserFromClaimAsync();
            var cartBooks = await _unitOfWork.CartBookRepository.GetCartBooksByUserIdAsync(user.UserId);

            // Check if all books in cart are available in inventory
            foreach (var cartBook in cartBooks)
            {
                var inventory = await _unitOfWork.InventoryRepository.GetInventoryByBookIdAsync(cartBook.BookId);
                if (inventory == null || inventory.Quantity == 0)
                {
                    // Book not available in inventory
                    return $"Book with ID {cartBook.BookId} is not available in inventory.";
                }
            }

            // All books are available, proceed with checkout
            foreach (var cartBook in cartBooks)
            {
                // Remove book from inventory
                var inventory = await _unitOfWork.InventoryRepository.GetInventoryByBookIdAsync(cartBook.BookId);
                inventory.Quantity = --inventory.Quantity;
               await _unitOfWork.InventoryRepository.UpdateAsync(inventory);

                // Remove book from cart
                await _unitOfWork.CartBookRepository.DeleteAsync(cartBook);
            }

            await _unitOfWork.CompleteAsync();

            return "Checkout successful.";
        }


    }
}
