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

namespace CleanArchitecture.Application.Project.Queries.GetProjectList
{
    public class GetProjectListQuery : IRequest<List<GetProjectListDTO>>
    {
        public class GetProjectListQueryHandler : IRequestHandler<GetProjectListQuery, List<GetProjectListDTO>>
        {
            IApplicationDbContext _context;
            IMapper _mapper;
            public GetProjectListQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<GetProjectListDTO>> Handle(GetProjectListQuery request, CancellationToken cancellationToken)
            {
                var vm = new List<GetProjectListDTO>();

                vm = await _context.Project
                    .ProjectTo<GetProjectListDTO>(_mapper.ConfigurationProvider)
                    .OrderBy(t => t.Name)
                    .ToListAsync(cancellationToken);

                return vm;

            }
        }
    }
}
