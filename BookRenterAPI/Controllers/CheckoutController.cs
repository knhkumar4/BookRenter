using BookRenterService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookRenter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService ?? throw new ArgumentNullException(nameof(checkoutService));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<string>))]
        public async Task<ActionResult<ApiResponse<string>>> CheckoutBooksAsync()
        {
            try
            {
                var result = await _checkoutService.CheckoutBooksAsync();
                if (result.Success)
                {
                    return Ok(new ApiResponse<string>(true, result.Message));
                }
                else
                {
                    return BadRequest(new ApiResponse<string>(false, result.Message));
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new ApiResponse<string>(false, "An error occurred while processing the checkout."));
            }
        }

    }
}
