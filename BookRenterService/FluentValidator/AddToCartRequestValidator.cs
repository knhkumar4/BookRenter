using BookRenter.Models.Requests;
using FluentValidation;


namespace BookRenterService.FluentValidator
{
 
    public class AddToCartRequestValidator : AbstractValidator<AddToCartRequest>
    {
        public AddToCartRequestValidator()
        {
            RuleFor(x => x.BookId).GreaterThan(0).WithMessage("BookId must be greater than 0.");
        }
    }

}
