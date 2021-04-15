using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.SharedKernel.Helper;
using CleanArchitecture.SharedKernel.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.User.Commands.Employee.Update
{
    public class UpdateEmployeeCommand : IRequest<int>
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public UpdateEmployeeCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }
            public async Task<int> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
            {
                AppUser user, currentUser;

                currentUser = _context.User.Include(x=>x.UserList).FirstOrDefault(x => x.Id == _currentUserService.UserId);

                user = currentUser.UserList.FirstOrDefault(x => x.Id == request.Id);

                if(user == null)
                    throw new NotFoundException(nameof(AppUser),request.Id);

                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.PhoneNumber = request.PhoneNumber;

                return await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
