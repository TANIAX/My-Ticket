using CleanArchitecture.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.User.Commands.Captcha
{
    public class CaptchaCommandValidator : AbstractValidator<CaptchaCommand>
    {

        public CaptchaCommandValidator()
        {

            RuleFor(v => v.Token)
                .NotEmpty().WithMessage("Token is required.");
        }
    }
}
