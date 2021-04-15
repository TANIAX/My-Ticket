using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;

namespace CleanArchitecture.Application.Ticket.Commands.CreateStaff
{
    public class CreateStaffTicketCommandValidator : AbstractValidator<CreateStaffTicketCommand>
    {
        public CreateStaffTicketCommandValidator()
        {
            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Title is required")
                .MinimumLength(5).WithMessage("Title must exceed 5 characters")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters");

            RuleFor(v => v.Description)
                .NotEmpty().WithMessage("Description is required")
                .MinimumLength(25).WithMessage("Description must exceed 25 characters");

            RuleFor(v => v.Project)
                .NotEmpty().WithMessage("You need to select a project")
                .MustAsync(HigherThan0).WithMessage("You need to select a project");

            RuleFor(v => v.Priority)
                .NotEmpty().WithMessage("You need to select a priority")
                .MustAsync(HigherThan0).WithMessage("You need to select a priority");

            RuleFor(v => v.Type)
                .NotEmpty().WithMessage("You need to select a type")
                .MustAsync(HigherThan0).WithMessage("You need to select a type");
        }

        private async Task<bool> HigherThan0(int number, CancellationToken cancellationToken)
        {
            if (number > 0)
                return true;
            else
                return false;
        }
    }
}
