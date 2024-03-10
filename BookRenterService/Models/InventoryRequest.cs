using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRenterService.Models
{
    public class InventoryRequest
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
        
    }
}
