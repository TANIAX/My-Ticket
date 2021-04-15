using CleanArchitecture.Application.Common.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CleanArchitecture.Application.User.Commands.Employee.Update;

namespace CleanArchitecture.Application.User.Commands.Employee.Update
{
    public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        private readonly IEmail _email;
        public UpdateEmployeeCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Id is required");

            RuleFor(v => v.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MinimumLength(3).WithMessage("First name must exceed 3 characters")
                .MaximumLength(20).WithMessage("First name must not exceed 20 characters");

            RuleFor(v => v.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MinimumLength(3).WithMessage("Last name must exceed 3 characters")
                .MaximumLength(20).WithMessage("Last name must not exceed 20 characters");

        }
    }
}
