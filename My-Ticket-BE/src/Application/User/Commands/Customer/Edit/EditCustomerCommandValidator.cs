using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.User.Commands.Employee.Create;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CleanArchitecture.Application.User.Commands.Customer.Edit
{
    public class EditCustomerCommandValidator : AbstractValidator<EditCustomerCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IEmail _email;
        public EditCustomerCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MinimumLength(3).WithMessage("First name must exceed 3 characters")
                .MaximumLength(20).WithMessage("First name must not exceed 20 characters");

            RuleFor(v => v.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MinimumLength(3).WithMessage("Last name must exceed 3 characters")
                .MaximumLength(20).WithMessage("Last name must not exceed 20 characters");

            RuleFor(v => v.CompanyName)
                .NotEmpty().WithMessage("Company name is required")
                .MinimumLength(3).WithMessage("Company name  must exceed 3 characters")
                .MaximumLength(20).WithMessage("Company name  must not exceed 20 characters");

        }
    }
}
