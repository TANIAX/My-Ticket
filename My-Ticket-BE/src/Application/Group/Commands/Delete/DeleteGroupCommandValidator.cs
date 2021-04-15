using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace CleanArchitecture.Application.Group.Commands.Delete
{
    public class DeleteGroupCommandValidator : AbstractValidator<DeleteGroupCommand>
    {
        public DeleteGroupCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}
