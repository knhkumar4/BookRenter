using BookRenterService.FluentValidator;
using BookRenterService.Interfaces;
using BookRenterService.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookRenterAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }


        [HttpPost]
        public async Task<IActionResult> Register(UserRequest userRequest)
        {
            // Validate the request
            var validationResult = await new UserRequestValidator().ValidateAsync(userRequest);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.First().ErrorMessage);
            }

            try
            {
                var createdUser = await _userService.CreateUserAsync(userRequest);
                return Ok(createdUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating user: {ex.Message}");
            }
        }

    }
}
