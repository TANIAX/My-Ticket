using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.SharedKernel.Helper;
using CleanArchitecture.SharedKernel.Identity;
using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityResult = Microsoft.AspNetCore.Identity.IdentityResult;

namespace CleanArchitecture.Application.User.Commands.Employee.ResetPassword
{
    public class ResetEmployeePasswordCommand : IRequest<int>
    {
        public string Id { get; set; }

        public class ResetEmployeePasswordCommandHandler : IRequestHandler<ResetEmployeePasswordCommand, int>
        {
            private readonly IIdentityService _userManager;
            private readonly IEmail _email;
            private readonly ICurrentUserService _currentUserService;
            private readonly IApplicationDbContext _context;

            public ResetEmployeePasswordCommandHandler(IIdentityService userManager, IEmail email,
                ICurrentUserService currentUserService, IApplicationDbContext context)
            {
                _userManager = userManager;
                _email = email;
                _currentUserService = currentUserService;
                _context = context;
            }

            public async Task<int> Handle(ResetEmployeePasswordCommand request, CancellationToken cancellationToken)
            {
                ApplicationUser currentUser;
                AppUser currentAppUser, appEmployerUser;
                IdentityResult result;
                string password;

                currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
                currentAppUser = await _context.User.Include(x=>x.UserList).FirstOrDefaultAsync(x => x.Id == currentUser.Id);
                appEmployerUser = currentAppUser.UserList.FirstOrDefault(x => x.Id == request.Id);

                if ( appEmployerUser != null)
                {
                    password = Helper.GeneratePassword(8);
                    result = await _userManager.SetPassword(request.Id, password);

                    if (!result.Succeeded)
                        throw new ApplicationException(result.Errors.ToList().ToString());
                    else
                    {
                        _email.SendEmail(appEmployerUser.Email,"Password has been reset", "Your password has been reset by " + currentAppUser.FirstName + currentAppUser.LastName +".<br>" +
                                                                    "For connecting again to My-Ticket, please use this one instead : " +password+"<br>"+
                                                                    "Kinds regards. <br>My-Ticket team.");
                        return 1;
                    }
                        
                }

                return 0;
            }
        }
    }
}
