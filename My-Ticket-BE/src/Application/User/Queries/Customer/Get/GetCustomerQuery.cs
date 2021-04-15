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

namespace CleanArchitecture.Application.User.Queries.Customer.Get
{
    public class GetCustomerQuery :IRequest<GetCustomerDTO>
    {
        public string Id { get; set; }
        public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, GetCustomerDTO>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            private readonly IMapper _mapper;
            
            public GetCustomerQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
            {
                _context = context;
                _currentUserService = currentUserService;
                _mapper = mapper;
            }

            public async Task<GetCustomerDTO> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
            {
                var vm = new GetCustomerDTO();

                vm = await _context.User
                    .Where(x=>x.Id == request.Id)
                    .ProjectTo<GetCustomerDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);

                return vm;
            }
        }
    }
}
