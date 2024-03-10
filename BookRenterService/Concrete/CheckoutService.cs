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

        public CheckoutService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> AddBookToCartAsync(int bookId, int cartId)
        {
            // Check if the cart already contains the book
            var existingCartItem = await _unitOfWork.CartBookRepository.GetByBookIdAndCartIdAsync(bookId, cartId);
            if (existingCartItem != null)
            {
                // Book already exists in the cart
                return false;
            }

            // Check if the cart already has 5 books
            var cartItemCount = await _unitOfWork.CartBookRepository.GetCartItemCountAsync(cartId);
            if (cartItemCount >= 5)
            {
                // Cart has reached the maximum limit
                throw new InvalidOperationException("A maximum of 5 books can be added to the cart.");
            }
            // Add the book to the cart
            var newCartItem = new CartBook
            {
                BookId = bookId,                
                CreatedDate = DateTime.Now
            };
            await _unitOfWork.CartBookRepository.AddAsync(newCartItem);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
