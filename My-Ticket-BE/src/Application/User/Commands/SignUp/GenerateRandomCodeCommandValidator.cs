using CleanArchitecture.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.User.Commands.SignUp
{
    class GenerateRandomCodeCommandValidator : AbstractValidator<GenerateRandomCodeCommand>
    {
        IEmail _email;
        public GenerateRandomCodeCommandValidator(IEmail email)
        {
            _email = email;
            
            RuleFor(v => v.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(255).WithMessage("Email must not exceed 255 characters.")
                .MinimumLength(3).WithMessage("Email must exceed 3 characters.")
                .Must(ValidMail).WithMessage("Email is not valid.");
        }

        private bool ValidMail(string email)
        {
            return _email.MailIsValid(email);
        }
    }
}
