using CleanArchitecture.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.User.Commands.SignUp
{
    public class EmailExistCommandValidator : AbstractValidator<EmailExistCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IEmail _email;
        public EmailExistCommandValidator(IApplicationDbContext context,IEmail email)
        {
            _context = context;
            _email = email;

            RuleFor(v => v.Email)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .MustAsync(BeUniqueEmail).WithMessage("Email already taken.");
        }
        public async Task<bool> BeUniqueEmail(string Email, CancellationToken cancellationToken)
        {
            if (await _context.User.FirstOrDefaultAsync(x => x.UserName == Email) == null)
                return true;
            else
                return false;
        }
    }
}
