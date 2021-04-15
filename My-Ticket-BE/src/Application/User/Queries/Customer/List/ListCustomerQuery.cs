using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.User.Queries.Employee.List;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.SharedKernel.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.User.Queries.Customer.List
{
    public class ListCustomerQuery : IRequest<List<CustomerListDTO>>
    {
        public class ListCustomerQueryHandler : IRequestHandler<ListCustomerQuery, List<CustomerListDTO>>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            private readonly IMapper _mapper;
            private readonly IIdentityService _userManager;
            public ListCustomerQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IIdentityService userManager, IMapper mapper)
            {
                _context = context;
                _currentUserService = currentUserService;
                _mapper = mapper;
                _userManager = userManager;
            }

            public async Task<List<CustomerListDTO>> Handle(ListCustomerQuery request, CancellationToken cancellationToken)
            {
                List<ApplicationUser> aul = new List<ApplicationUser>();
                List<AppUser> ul = new List<AppUser>();
                AppUser u;

                var vm = new List<CustomerListDTO>();

                vm = await _context.User
                .Where(x=> x.IsCompany == true)
                .ProjectTo<CustomerListDTO>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

                return vm;
            }
        }
    }
}
