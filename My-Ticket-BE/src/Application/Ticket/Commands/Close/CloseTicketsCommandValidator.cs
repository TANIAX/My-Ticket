using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Ticket.Commands.Close
{
    public class CloseTicketsCommandValidator : AbstractValidator<CloseTicketsCommand>
    {
        public CloseTicketsCommandValidator()
        {
            RuleFor(v => v.TicketList)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}
