
using BookRenter.Services;
using BookRenter.Services.Interfaces;
using BookRenterData.Entities;
using BookRenterService.Folder.BookRenter.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookRenter.Controllers
{
    [ApiController]
    [Route("api/books")]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookResponse>> GetBookById(int id)
        {
            var bookResponse = await _bookService.GetBookResponseByIdAsync(id);
            if (bookResponse == null)
            {
                return NotFound();
            }
            return Ok(bookResponse);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookResponse>>> GetAllBookResponses()
        {
            var bookResponses = await _bookService.GetAllBookResponsesAsync();
            return Ok(bookResponses);
        }

        [HttpPost]
        public async Task<ActionResult<BookResponse>> AddBook(BookResponse bookResponse)
        {
            var addedBookResponse = await _bookService.AddBookResponseAsync(bookResponse);
            return CreatedAtAction(nameof(GetBookById), new { id = addedBookResponse.BookId }, addedBookResponse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookResponse>> UpdateBook(int id, BookResponse bookResponse)
        {
            try
            {
                // Check if the provided book ID matches the ID in the request body
                if (id != bookResponse.BookId)
                {
                    return BadRequest("Book ID in the request body does not match the ID in the URL.");
                }

                // Update the book using the service
                var updatedBookResponse = await _bookService.UpdateBookResponseAsync(id, bookResponse);

                // Return the updated book response
                return Ok(updatedBookResponse);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while updating the book.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBookResponse(int id)
        {
            await _bookService.DeleteBookResponseAsync(id);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BookResponse>>> SearchBooksAsync([FromQuery] string searchTerm)
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
