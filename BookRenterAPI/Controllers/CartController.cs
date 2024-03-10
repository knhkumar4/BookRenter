using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookRenter.Models.Requests;
using BookRenter.Services.Interfaces;
using BookRenterService.Models;
using BookRenterService.Interfaces;
using Microsoft.AspNetCore.Authorization;

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
            try
            {
                var isSuccess = await _cartService.AddBookToCartAsync(request.BookId);

                if (isSuccess)
                {
                    return Created(string.Empty, new ApiResponse<string>(true, "Book added to cart successfully."));
                }
                else
                {
                    return BadRequest(new ApiResponse<string>(false, "The book is already in the cart."));
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new ApiResponse<string>(false, "An error occurred while processing the request."));
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
