using BookRenter.Models.Responses;
using FluentValidation;
using System;

namespace BookRenterService.FluentValidator
{   
    public class BookRequestValidator : AbstractValidator<BookRequest>
    {
        public BookRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(255).WithMessage("Title must not exceed 255 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

            RuleFor(x => x.Author)
                .NotEmpty().WithMessage("Author is required.")
                .MaximumLength(255).WithMessage("Author must not exceed 255 characters.");

            RuleFor(x => x.Genre)
                .MaximumLength(100).WithMessage("Genre must not exceed 100 characters.");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Price is required.")
                .Must(BeADecimal).WithMessage("Price must be a valid integer.")
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.RentPrice)
                 .NotEmpty().WithMessage("RentPrice is required.")
                 .Must(BeANullableDecimal).WithMessage("Price must be a valid integer.")
                .GreaterThan(0).When(x => x.RentPrice != null).WithMessage("Rent price must be greater than 0.");

           
        }
        private bool BeADecimal(double price)
        {
            return true;
        }
        private bool BeANullableDecimal(double? price)
        {
            return true;
        }
    }

}
