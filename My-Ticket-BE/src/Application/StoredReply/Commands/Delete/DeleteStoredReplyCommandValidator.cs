using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.StoredReply.Commands.Delete;
using CleanArchitecture.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.StoredReply.Commands.Delete
{
    public class DeleteStoredReplyCommandValidator : AbstractValidator<DeleteStoredReplyCommand>
    {
        public DeleteStoredReplyCommandValidator()
        {
            RuleFor(v => v.storedReplyList)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}
