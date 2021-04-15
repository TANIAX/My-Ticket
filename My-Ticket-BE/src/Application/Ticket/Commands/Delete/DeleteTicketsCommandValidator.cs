using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Ticket.Commands.Delete
{
    public class DeleteTicketsCommandValidator : AbstractValidator<DeleteTicketsCommand>
    {
        public DeleteTicketsCommandValidator()
        {
            RuleFor(v => v.TicketList)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}
