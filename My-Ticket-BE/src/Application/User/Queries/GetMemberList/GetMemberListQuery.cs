using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.User.Queries.MyAccount;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.SharedKernel.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.User.Queries.GetMemberList
{
    public class GetMemberListQuery : IRequest<List<GetMemberListDTO>>
    {
        public class GetMemberListHandler : IRequestHandler<GetMemberListQuery, List<GetMemberListDTO>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IIdentityService _userManager;

            public GetMemberListHandler(IApplicationDbContext context, IMapper mapper, IIdentityService userManager)
            {
                _context = context;
                _mapper = mapper;
                _userManager = userManager;
            }

            public async Task<List<GetMemberListDTO>> Handle(GetMemberListQuery request, CancellationToken cancellationToken)
            {
                AppUser appUser = null;
                var vm = new List<GetMemberListDTO>();
                var memberList = await _userManager.UsersInRoleAsync("Member") as List<ApplicationUser>;
                foreach (var user in memberList)
                {
                    appUser = await _context.User.FirstOrDefaultAsync(x => x.Id == user.Id);
                    vm.Add(new GetMemberListDTO
                    {
                        Id = appUser.Id,
                        LastName = appUser.LastName,
                        FirstName = appUser.FirstName,
                        Email = appUser.Email
                    });
                }

                return vm;
            }
        }
    }
}
