
using BookRenter.Services;
using BookRenterService.Folder.BookRenter.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BookRenter.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private readonly BookResponseService _bookResponseService;

        public BookController(BookResponseService bookResponseService)
        {
            _bookResponseService = bookResponseService ?? throw new ArgumentNullException(nameof(bookResponseService));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookResponse>> GetBookById(int id)
        {
            var bookResponse = await _bookResponseService.GetBookResponseByIdAsync(id);
            if (bookResponse == null)
            {
                return NotFound();
            }
            return Ok(bookResponse);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookResponse>>> GetAllBookResponses()
        {
            var bookResponses = await _bookResponseService.GetAllBookResponsesAsync();
            return Ok(bookResponses);
        }

        [HttpPost]
        public async Task<ActionResult<BookResponse>> AddBook(BookResponse bookResponse)
        {
            var addedBookResponse = await _bookResponseService.AddBookResponseAsync(bookResponse);
            return CreatedAtAction(nameof(GetBookById), new { id = addedBookResponse.BookId }, addedBookResponse);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBookResponse(int id)
        {
            await _bookResponseService.DeleteBookResponseAsync(id);
            return NoContent();
        }
    }
}
