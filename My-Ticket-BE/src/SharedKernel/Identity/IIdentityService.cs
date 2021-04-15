using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.SharedKernel.Identity
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);

        Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

        Task<Result> DeleteUserAsync(string userId);
        Task<ApplicationUser> FindByIdAsync(string userId);
        Task<IdentityResult> SetPassword(string userId, string password);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        Task<IdentityResult> DeleteAsync(ApplicationUser user);
        string IdentityResultError(IdentityResult result);
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
        Task<bool> IsInRoleAsync(string userId, string role);
        Task<IList<ApplicationUser>> UsersInRoleAsync(string role);
    }
}
