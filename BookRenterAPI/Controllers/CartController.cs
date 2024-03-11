using Microsoft.AspNetCore.Mvc;
using BookRenter.Models.Requests;
using BookRenterService.Models;
using BookRenterService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using BookRenterService.FluentValidator;

namespace BookRenter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CartBookResponse>>> GetAllCartItemsAsync()
        {
            var cartItems = await _cartService.GetAllCartItemsAsync();
            return Ok(cartItems); // 200 OK with the cart items
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            // Validate the request
            var validationResult = await new AddToCartRequestValidator().ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ApiResponse<string>(false, validationResult.Errors.First().ErrorMessage));
            }

            var (success, message) = await _cartService.AddBookToCartAsync(request.BookId);

            if (success)
            {
                return Created(string.Empty, new ApiResponse<string>(true, message));
            }
            else
            {
                return BadRequest(new ApiResponse<string>(false, message));
            }
        }


        [HttpDelete("{bookId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveBookFromCartAsync(int bookId)
        {
            var success = await _cartService.RemoveBookFromCartAsync(bookId);
            if (success)
            {
                return NoContent(); // 204 No Content
            }
            else
            {
                return NotFound(new ApiResponse<string>(false, "Book not found in the cart.")); // 404 Not Found
            }
        }
    }
}
