using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.User.Queries.Employee.List
{
    public class EmployeeListQuery : IRequest<MyEmployeesDTO>
    {
        public class MyEmployeesQueryHandler : IRequestHandler<EmployeeListQuery, MyEmployeesDTO>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            private readonly IMapper _mapper;
            public MyEmployeesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
            {
                _context = context;
                _currentUserService = currentUserService;
                _mapper = mapper;
            }

            public async Task<MyEmployeesDTO> Handle(EmployeeListQuery request, CancellationToken cancellationToken)
            {
                var vm = new MyEmployeesDTO();

                vm = await _context.User
                    .Where(x => x.Id == _currentUserService.UserId)
                .ProjectTo<MyEmployeesDTO>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

                return vm;
            }
        }
    }
}
