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

namespace CleanArchitecture.Application.Priority.Queries.GetPriorityList
{
    public class GetPriorityListQuery : IRequest<List<GetPriorityListDTO>>
    {
        public class GetPriorityListQueryHandler : IRequestHandler<GetPriorityListQuery, List<GetPriorityListDTO>>
        {
            IApplicationDbContext _context;
            IMapper _mapper;
            public GetPriorityListQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<GetPriorityListDTO>> Handle(GetPriorityListQuery request, CancellationToken cancellationToken)
            {
                var vm = new List<GetPriorityListDTO>();

                vm = await _context.Priority
                    .ProjectTo<GetPriorityListDTO>(_mapper.ConfigurationProvider)
                    .OrderBy(t => t.Name)
                    .ToListAsync(cancellationToken);

                return vm;

            }
        }
    }
}
