using BookRenterData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRenterService.Models
{
    public class SearchBookResponse
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public double Price { get; set; }
        public double? RentPrice { get; set; }

        public static implicit operator SearchBookResponse(Book book)
        {
            if (book == null)
            {
                return null;
            }

            return new SearchBookResponse
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
    }
}
