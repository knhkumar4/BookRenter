using System;
using System.Threading.Tasks;
using BookRenter.Services.Interfaces;
using BookRenterData.Entities;
using BookRenterData.UnitOfWork.Interfaces;
using BookRenterService.Interfaces;
using BookRenterService.Models;

namespace BookRenter.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserClaimService _userClaimService;

        public CartService(IUnitOfWork unitOfWork, IUserClaimService userClaimService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userClaimService = userClaimService ?? throw new ArgumentNullException(nameof(userClaimService));
        }

        public async Task<(bool Success, string Message)> AddBookToCartAsync(int bookId)
        {
            var user = await _userClaimService.GetUserFromClaimAsync();
            var userId = user.UserId;

            var existingCartItem = await _unitOfWork.CartBookRepository.GetByBookIdAndUserIdAsync(bookId, userId);
            if (existingCartItem != null)
            {
                return (false, "The book is already in the cart.");
            }

            var cartItemCount = await _unitOfWork.CartBookRepository.GetCartItemCountAsync(userId);
            if (cartItemCount >= 5)
            {
                return (false, "A maximum of 5 books can be added to the cart.");
            }

            var newCartItem = new CartBook
            {
                BookId = bookId,
                Quantity = 1,
                UserId = userId,
                CreatedDate = DateTime.UtcNow
            };
            await _unitOfWork.CartBookRepository.AddAsync(newCartItem);
            await _unitOfWork.CompleteAsync();
            return (true, "Book added to cart successfully.");
        }


        public async Task<bool> RemoveBookFromCartAsync(int bookId)
        {
            var user = await _userClaimService.GetUserFromClaimAsync();
            var userId = user.UserId;

            // Check if the book exists in the cart
            var cartItem = await _unitOfWork.CartBookRepository.GetByBookIdAndUserIdAsync(bookId, userId);
            if (cartItem == null)
            {
                // Book does not exist in the cart
                return false;
            }

            // Remove the book from the cart
            await _unitOfWork.CartBookRepository.DeleteAsync(cartItem);
            await _unitOfWork.CompleteAsync();

            return true;
        }
        public async Task<IEnumerable<CartBookResponse>> GetAllCartItemsAsync()
        {
            var user = await _userClaimService.GetUserFromClaimAsync();
            var userId = user.UserId;

            // Retrieve all cart items for the user
            var cartBooks = await _unitOfWork.CartBookRepository.GetByUserIdAsync(userId);

            // Map CartBook objects to CartBookResponse objects
            var cartBookResponses = cartBooks.Select(cb => (CartBookResponse)cb.Book);

            return cartBookResponses;
        }

    }
}
