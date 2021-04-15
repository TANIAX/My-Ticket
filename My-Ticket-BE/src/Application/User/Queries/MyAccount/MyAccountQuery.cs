using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.User.Queries.MyAccount
{
    public class MyAccountQuery : IRequest<MyAccountDTO>
    {
        public class GetMyAccountHandler : IRequestHandler<MyAccountQuery, MyAccountDTO>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly ICurrentUserService _currentUserService;

            public GetMyAccountHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
            {
                _context = context;
                _mapper = mapper;
                _currentUserService = currentUserService;
            }

            public async Task<MyAccountDTO> Handle(MyAccountQuery request, CancellationToken cancellationToken)
            {
                var vm = new MyAccountDTO();

                vm = await _context.User
                    .Where(x => x.Id == _currentUserService.UserId)
                .ProjectTo<MyAccountDTO>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

                return vm;
            }
        }
    }
}
