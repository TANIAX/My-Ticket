using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using CleanArchitecture.SharedKernel.Helper;
using CleanArchitecture.SharedKernel.Identity;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.User.Commands.Captcha
{
    public partial class CaptchaCommand : IRequest<HttpResponseMessage>
    {
        public string Token { get; set; }

        public class CaptchaCommandHandler : IRequestHandler<CaptchaCommand, HttpResponseMessage>
        {
            private readonly ICaptcha _captcha;

            public CaptchaCommandHandler(ICaptcha captcha)
            {
                _captcha = captcha;
            }

            public async Task<HttpResponseMessage> Handle(CaptchaCommand request, CancellationToken cancellationToken)
            {
                var response = _captcha.GetAsync(request.Token);
                if (response.Result.IsSuccessStatusCode)//FIX ME After sent a bearer token, it should not pass into it but it do, so check by another way
                {
                    return await response;
                }
                else
                {
                    throw new ApplicationException();
                }
            }

        }
    }
}
