using BookRenterData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRenterService.Models
{
    public class CartBookResponse
    {
        public int BookId { get; set; }
        public string BookName { get; set; }

        // Implicit conversion from Book to CartBookResponse
        public static implicit operator CartBookResponse(Book book)
        {
            return new CartBookResponse
            {
                BookId = book.BookId,
                BookName = book.Title // Assuming "Title" is the property representing the book name
            };
        }
    }


}
