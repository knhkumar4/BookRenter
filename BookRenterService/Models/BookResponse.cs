﻿
using BookRenterData.Entities;

namespace BookRenter.Models.Responses
{
    public class BookResponse
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public double Price { get; set; }
        public double? RentPrice { get; set; }
     

        public static implicit operator BookResponse(Book book)
        {
            if (book == null)
            {
                return null;
            }

            return new BookResponse
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

        public static implicit operator Book(BookResponse bookResponse)
        {
            if (bookResponse == null)
            {
                return null;
            }

            return new Book
            {
                BookId = bookResponse.BookId,
                Title = bookResponse.Title,
                Author = bookResponse.Author,
                Description = bookResponse.Description,
                Genre = bookResponse.Genre,
                Price = bookResponse.Price,
                CreatedDate = DateTime.UtcNow,
                RentPrice = bookResponse.RentPrice

            };
        }
    }
}

