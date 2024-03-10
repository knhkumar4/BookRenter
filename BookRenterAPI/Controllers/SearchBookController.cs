using BookRenter.Services.Interfaces;
using BookRenterService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookRenter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SearchBookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public SearchBookController(IBookService bookService)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        }

        /// <summary>
        /// Searches for books using the specified search term.
        /// </summary>
        /// <param name="searchTerm">Search a book by its name, by its Author.</param>
        /// <returns>A list of book search results.</returns>
        /// <response code="200">Returns the search results.</response>
        /// <response code="400">If the search term is empty.</response>
        /// <response code="500">If an error occurs while searching for books.</response>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<SearchBookResponse>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<SearchBookResponse>>> SearchBooksAsync([FromQuery] string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return BadRequest("Search term cannot be empty");
                }

                var searchResults = await _bookService.SearchBooksAsync(searchTerm);
                return Ok(searchResults);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while searching for books.");
            }
        }
    }
}
