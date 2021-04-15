using CleanArchitecture.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.User.Commands.ResetPassword
{
    class UploadPPCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        private readonly IApplicationDbContext _context;
        public UploadPPCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(255).WithMessage("Email must not exceed 255 characters.")
                .MinimumLength(3).WithMessage("Email must exceed 3 characters.")
                .Must(EmailExist);
        }

        private bool EmailExist(string email)
        {
            if (_context.User.FirstAsync(x => x.UserName == email) != null)
                return true;
            else
                return false;
        }
    }
}
