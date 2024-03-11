using BookRenterService.Models;
using FluentValidation;

namespace BookRenterService.FluentValidator
{
    public class InventoryRequestValidator : AbstractValidator<InventoryRequest>
    {
        public InventoryRequestValidator()
        {
            RuleFor(x => x.BookId)
                .NotEmpty().WithMessage("BookId is required.");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}
