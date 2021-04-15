using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.User.Commands.SignUp
{
    public class EmailExistCommand : IRequest<bool>
    {
        public string Email { get; set; }
        public class EmailExistCommandHandler : IRequestHandler<EmailExistCommand, bool>
        {
            public EmailExistCommandHandler()
            {

            }

            public async Task<bool> Handle(EmailExistCommand request, CancellationToken cancellationToken)
            {
                return true;
            }
        }
    }
}
