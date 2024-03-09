namespace BookRenter.Models.Requests
{
    public class AddToCartRequest
    {
        public int BookId { get; set; }
        public int CartId { get; set; }
    }
}
