
using BookRenterData.Entities;

namespace BookRenter.Models.Responses
{
    public class BookRequest
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public double Price { get; set; }
        public double? RentPrice { get; set; }      

        public static implicit operator BookRequest(Book book)
        {
            if (book == null)
            {
                return null;
            }

            return new BookRequest
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                Genre = book.Genre,
                Price = book.Price,
                RentPrice = book.RentPrice,

            };
        }

        public static implicit operator Book(BookRequest bookResponseRequest)
        {
            if (bookResponseRequest == null)
            {
                return null;
            }

            return new Book
            {
                BookId = bookResponseRequest.BookId,
                Title = bookResponseRequest.Title,
                Author = bookResponseRequest.Author,
                Description = bookResponseRequest.Description,
                Genre = bookResponseRequest.Genre,
                Price = bookResponseRequest.Price,
                CreatedDate = DateTime.UtcNow,
                RentPrice = bookResponseRequest.RentPrice

            };
        }
    }
}

