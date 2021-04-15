using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using CleanArchitecture.SharedKernel.GlobalVar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.SharedKernel.Identity;

namespace CleanArchitecture.Application.User.Commands.User.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<bool>
    {
        public string Email { get; set; }
        public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand,bool>
        {
            private readonly IEmail _email;
            private readonly IIdentityService _userManager;
            public ForgotPasswordCommandHandler(IIdentityService userManager, IEmail email)
            {
                _email = email;
                _userManager = userManager;
            }

            public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
            {
                ApplicationUser user;
                string code;
                string callbackUrl;

                user = await _userManager.FindByEmailAsync(request.Email);
                if(user == null)
                    return true;

                code = await _userManager.GeneratePasswordResetTokenAsync(user);
                // We replace the char / for prevent getting multiple params to our front end
                var cide = code;
                code = code.Replace("/", "{");

                callbackUrl = GlobalVar.FrontendBaseUrl + "User/ResetPassword/" + code;

                _email.SendEmail(request.Email, "Reset Password",
                "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a><p>"+cide+"</p>");

                return true;
            }
        }
    }
}
