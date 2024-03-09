using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BookRenter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookRenter.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpPost]
        public async Task<ActionResult<Book>> AddBook(Book book)
        {
            var addedBook = await _bookService.AddBookAsync(book);
            return CreatedAtAction(nameof(GetBookById), new { id = addedBook.BookId }, addedBook);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            await _bookService.DeleteBookAsync(book);
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteBooks(Expression<Func<Book, bool>> filter)
        {
            await _bookService.DeleteBooksAsync(filter);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Book>>> SearchBooks([FromQuery] string title, [FromQuery] string author)
        {
            // You can implement search functionality based on title and author here
            // For simplicity, let's assume it returns all books if no search parameters are provided
            if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(author))
            {
                return await GetAllBooks();
            }

            // Implement your search logic here
            // For example:
            // var books = await _bookService.SearchBooksAsync(title, author);
            // return Ok(books);

            return NotFound();
        }
    }
}
