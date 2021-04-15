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

namespace CleanArchitecture.Application.Type.Queries.GetTypeList
{
    public class GetTypeListQuery : IRequest<List<GetTypeListDTO>>
    {
        public class GetTypeListQueryHandler : IRequestHandler<GetTypeListQuery, List<GetTypeListDTO>>
        {
            IApplicationDbContext _context;
            IMapper _mapper;
            public GetTypeListQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<GetTypeListDTO>> Handle(GetTypeListQuery request, CancellationToken cancellationToken)
            {
                var vm = new List<GetTypeListDTO>();

                vm = await _context.Type
                    .ProjectTo<GetTypeListDTO>(_mapper.ConfigurationProvider)
                    .OrderBy(t => t.Name)
                    .ToListAsync(cancellationToken);

                return vm;

            }
        }
    }
}
