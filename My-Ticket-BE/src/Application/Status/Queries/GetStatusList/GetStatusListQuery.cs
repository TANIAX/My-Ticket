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

namespace CleanArchitecture.Application.Status.Queries.GetStatusList
{
    public class GetStatusListQuery : IRequest <List<GetStatusListDTO>>
    {
        public class GetStatusListQueryHandler : IRequestHandler<GetStatusListQuery, List<GetStatusListDTO>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetStatusListQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<GetStatusListDTO>> Handle(GetStatusListQuery request, CancellationToken cancellationToken)
            {
                var vm = new List<GetStatusListDTO>();

                vm = await _context.Status
                    .ProjectTo<GetStatusListDTO>(_mapper.ConfigurationProvider)
                    .OrderBy(t => t.Name)
                    .ToListAsync(cancellationToken);

                return vm;
            }
        }
    }
}
