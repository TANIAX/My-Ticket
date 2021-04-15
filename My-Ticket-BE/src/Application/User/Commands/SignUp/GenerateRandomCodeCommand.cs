using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using CleanArchitecture.SharedKernel.GlobalVar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.User.Commands.SignUp
{
    public class GenerateRandomCodeCommand : IRequest<object>
    {
        public string Email { get; set; }
        public class GenerateRandomCodeCommandHandler : IRequestHandler<GenerateRandomCodeCommand, object>
        {
            private readonly IEmail _email;
            public GenerateRandomCodeCommandHandler(IEmail email)
            {
                _email = email;
            }

            public async Task<object> Handle(GenerateRandomCodeCommand request, CancellationToken cancellationToken)
            {
                Random random = new Random();
                int length = 5;
                string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                string code;

                code = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());

                try
                {
                    _email.SendEmail(request.Email, "Account confirmation", "<h1>Your code : " + code + "</h1>");
                }
                catch (Exception e)
                {
                    throw e;
                }
                return await Task.FromResult(new { codeConfirmation = code });
            }
        }
    }
}
