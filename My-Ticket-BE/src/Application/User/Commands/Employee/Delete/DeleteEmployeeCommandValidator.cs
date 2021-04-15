using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace CleanArchitecture.Application.User.Commands.Employee.Delete
{
    public class DeleteEmployeeCommandValidator : AbstractValidator<DeleteEmployeeCommand>
    {
        public DeleteEmployeeCommandValidator()
        {
            RuleFor(v => v.EmployeeList)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}
