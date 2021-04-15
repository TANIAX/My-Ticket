using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.StoredReply.Commands.Delete;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.SharedKernel.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.User.Commands.Employee.Delete
{
    public class DeleteEmployeeCommand : IRequest<int>
    {
        public string EmployeeList { get; set; }
        public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            private readonly IIdentityService _userManager;
            public DeleteEmployeeCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IIdentityService userManager)
            {
                _context = context;
                _currentUserService = currentUserService;
                _userManager = userManager;
            }

            public async Task<int> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
            {
                ApplicationUser currentUser, employeeUser;
                AppUser appCurrentUser, appEmployeeUser;
                string[] idTab;

                currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
                appCurrentUser = await _context.User.FirstOrDefaultAsync(x => x.Id == currentUser.Id);



                idTab = request.EmployeeList.Split(",");


                foreach (var id in idTab)
                {
                    employeeUser = await _userManager.FindByIdAsync(id);
                    if (employeeUser == null)
                        throw new NotFoundException(nameof(ApplicationUser), id);

                    appEmployeeUser = await _context.User.Include(x => x.UserList).FirstOrDefaultAsync(x => x.Id == id);
                    if (appEmployeeUser == null)
                        throw new NotFoundException(nameof(AppUser), id);

                    var result = await _userManager.DeleteAsync(employeeUser);
                    if (!result.Succeeded)
                        throw new ApplicationException(result.Errors.ToList().ToString());

                    _context.User.Remove(appEmployeeUser);

                }

                return await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}