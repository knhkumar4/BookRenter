using BookRenter.Models.Responses;
using BookRenter.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookRenter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        }

        /// <summary>
        /// Retrieves a book by its ID.
        /// </summary>
        /// <param name="id">The ID of the book.</param>
        /// <returns>The book with the specified ID.</returns>
        /// <response code="200">Returns the book with the specified ID.</response>
        /// <response code="404">If no book is found with the specified ID.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(BookResponse))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BookResponse>> GetBookById(int id)
        {
            var bookResponse = await _bookService.GetBookResponseByIdAsync(id);
            if (bookResponse == null)
            {
                return NotFound();
            }
            return Ok(bookResponse);
        }

        /// <summary>
        /// Retrieves all books.
        /// </summary>
        /// <returns>A list of all books.</returns>
        /// <response code="200">Returns the list of all books.</response>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookResponse>))]
        public async Task<ActionResult<IEnumerable<BookResponse>>> GetAllBooks()
        {
            var bookResponses = await _bookService.GetAllBookResponsesAsync();
            return Ok(bookResponses);
        }

        /// <summary>
        /// Adds a new book.
        /// </summary>
        /// <param name="bookRequest">The book to add.</param>
        /// <returns>The added book.</returns>
        /// <response code="201">Returns the added book.</response>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(BookResponse))]
        public async Task<ActionResult<BookResponse>> AddBook(BookRequest bookRequest)
        {
            var addedBookResponse = await _bookService.AddBookAsync(bookRequest);
            return CreatedAtAction(nameof(GetBookById), new { id = addedBookResponse.BookId }, addedBookResponse);
        }

        /// <summary>
        /// Updates an existing book.
        /// </summary>
        /// <param name="id">The ID of the book to update.</param>
        /// <param name="bookRequest">The updated book data.</param>
        /// <returns>The updated book.</returns>
        /// <response code="200">Returns the updated book.</response>
        /// <response code="400">If the provided book ID does not match the ID in the request body.</response>
        /// <response code="500">If an error occurs while updating the book.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(BookResponse))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<BookResponse>> UpdateBook(int id, BookRequest bookRequest)
        {
            try
            {
                // Check if the provided book ID matches the ID in the request body
                if (id != bookRequest.BookId)
                {
                    return BadRequest("Book ID in the request body does not match the ID in the URL.");
                }

                // Update the book using the service
                var updatedBookResponse = await _bookService.UpdateBookAsync(id, bookRequest);

                // Return the updated book response
                return Ok(updatedBookResponse);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while updating the book.");
            }
        }
    }
}
