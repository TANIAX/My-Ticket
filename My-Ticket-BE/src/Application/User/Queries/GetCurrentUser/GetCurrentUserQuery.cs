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

namespace CleanArchitecture.Application.User.Queries.GetCurrentUser
{
    public class GetCustomerQuery :IRequest<GetCurrentUserDTO>
    {
        public class GetCurrentUserQueryHandler : IRequestHandler<GetCustomerQuery, GetCurrentUserDTO>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            private readonly IMapper _mapper;
            
            public GetCurrentUserQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
            {
                _context = context;
                _currentUserService = currentUserService;
                _mapper = mapper;
            }

            public async Task<GetCurrentUserDTO> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
            {
                var vm = new GetCurrentUserDTO();

                vm = await _context.User.Include(x=>x.StoredReply).Include(x=>x.UserList).Include(x=>x.Group)
                    .Where(x=>x.Id == _currentUserService.UserId)
                    .ProjectTo<GetCurrentUserDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);

                return vm;
            }
        }
    }
}
