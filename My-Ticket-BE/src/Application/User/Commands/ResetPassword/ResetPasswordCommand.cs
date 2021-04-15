using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using CleanArchitecture.SharedKernel.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.User.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<bool>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
        public class ResetPasswordCommandHangler : IRequestHandler<ResetPasswordCommand, bool>
        {
            private readonly IEmail _email;
            private readonly IIdentityService _userManager;
            public ResetPasswordCommandHangler(IIdentityService userManager, IEmail email)
            {
                _email = email;
                _userManager = userManager;
            }

            public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
            {
                ApplicationUser user;
                IdentityResult result;

                user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    //return Request.CreateResponse(HttpStatusCode.NotFound, "User not found");
                    throw new NotFoundException("User not found", user);
                }

                result = await _userManager.ResetPasswordAsync(user, request.Code, request.Password);
                if (result.Succeeded)
                {

                    _email.SendEmail(request.Email, "Password is been reset",
                    "Your password is succefully reset");
                    return true;
                }
                else
                {
                    throw new ApplicationException("Token is invalid.");
                }
            }
        }
    }
}
