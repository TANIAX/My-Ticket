using CleanArchitecture.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace CleanArchitecture.WebUI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == "jti")?.Value;
        }

        public string UserId { get; }
    }
}
