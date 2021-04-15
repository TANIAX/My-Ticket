using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.StoredReply.Commands.Update
{
    public class UpdateStoredReplyCommandValidator : AbstractValidator<UpdateStoredReplyCommand>
    {
        public UpdateStoredReplyCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(50).WithMessage("Title must not exceed 50 characters.")
                .MinimumLength(5).WithMessage("Title must exceed 5 characters.");

            RuleFor(v => v.Reply)
                .NotEmpty().WithMessage("Reply is required");
        }
    }
}
