using BookRenterService.Models;
using FluentValidation;

namespace BookRenterService.FluentValidator
{
    public class InventoryRequestValidator : AbstractValidator<InventoryRequest>
    {
        public InventoryRequestValidator()
        {
            RuleFor(x => x.BookId)
             .NotEmpty().WithMessage("BookId is required.")
             .Must(BeAnInteger).WithMessage("BookId must be a valid integer.")
             .GreaterThan(0).WithMessage("BookId must be greater than 0.");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .Must(BeAnInteger).WithMessage("Quantity must be a valid integer.")
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }

        private bool BeAnInteger(int bookId)
        {
            return true;
        }
    }
}
