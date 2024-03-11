using BookRenter.Models.Requests;
using FluentValidation;


namespace BookRenterService.FluentValidator
{
 
    public class AddToCartRequestValidator : AbstractValidator<AddToCartRequest>
    {
        public AddToCartRequestValidator()
        {
            RuleFor(x => x.BookId)
             .NotEmpty().WithMessage("BookId is required.")
             .Must(BeAnInteger).WithMessage("BookId must be a valid integer.")
             .GreaterThan(0).WithMessage("BookId must be greater than 0.");
        }
        private bool BeAnInteger(int bookId)
        {
            return true;
        }
    }

}
