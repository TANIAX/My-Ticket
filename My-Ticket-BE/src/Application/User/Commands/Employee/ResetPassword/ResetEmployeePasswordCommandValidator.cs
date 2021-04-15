using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace CleanArchitecture.Application.User.Commands.Employee.ResetPassword
{
    public class ResetEmployeePasswordCommandValidator : AbstractValidator<ResetEmployeePasswordCommand>
    {
        public ResetEmployeePasswordCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}
