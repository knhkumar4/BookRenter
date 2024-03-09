using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRenterService.Folder
{
    using System;

    namespace BookRenter.Models.Responses
    {
        public class BookResponse
        {
            public int BookId { get; set; }

            public string Title { get; set; }

            public string Author { get; set; }

            public DateTime PublishedDate { get; set; }

            public string Description { get; set; }

            public string Genre { get; set; }

            public decimal Price { get; set; }
        }
    }

}
