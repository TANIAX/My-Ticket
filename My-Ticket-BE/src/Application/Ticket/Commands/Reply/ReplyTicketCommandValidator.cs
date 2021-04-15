using CleanArchitecture.Application.Common.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Ticket.Commands.Reply
{
    public class ReplyTicketCommandValidator : AbstractValidator<ReplyTicketCommand>
    {
        private readonly IEmail _email;
        public ReplyTicketCommandValidator(IEmail email)
        {
            _email = email;

            RuleFor(v => v.TicketHeaderId)
                .GreaterThan(0).WithMessage("Id is required");
        }
    }
}
