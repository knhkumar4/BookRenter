using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookRenter.Services.Interfaces;
using BookRenter.Models.Requests;
using BookRenter.Services;

namespace BookRenter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService ?? throw new ArgumentNullException(nameof(checkoutService));
        }

        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            try
            {
                var isSuccess = await _checkoutService.AddBookToCartAsync(request.BookId, request.CartId);

                if (isSuccess)
                {
                    return Ok("Book added to cart successfully.");
                }
                else
                {
                    return BadRequest("The book is already in the cart.");
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}
