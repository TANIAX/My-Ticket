using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;

namespace CleanArchitecture.Application.Ticket.Commands.CreateCustomer
{
    public class CreateCustomerTicketCommandValidator : AbstractValidator<CreateCustomerTicketCommand>
    {
        public CreateCustomerTicketCommandValidator()
        {
            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Title is required")
                .MinimumLength(5).WithMessage("Title must exceed 5 characters")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters");

            RuleFor(v => v.Description)
                .NotEmpty().WithMessage("Description is required")
                .MinimumLength(25).WithMessage("Description must exceed 25 characters");

        }
    }
}
