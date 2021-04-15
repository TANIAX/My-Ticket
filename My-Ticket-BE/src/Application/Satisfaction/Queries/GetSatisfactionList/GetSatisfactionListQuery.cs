using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Satisfaction.Queries.GetSatisfactionList
{
    public class GetSatisfactionListQuery : IRequest<List<GetSatisfactionListDTO>>
    {
        public class GetSatisfactionListQueryHandler : IRequestHandler<GetSatisfactionListQuery, List<GetSatisfactionListDTO>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetSatisfactionListQueryHandler(IApplicationDbContext context,IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<GetSatisfactionListDTO>> Handle(GetSatisfactionListQuery request, CancellationToken cancellationToken)
            {
                var vm = new List<GetSatisfactionListDTO>();

                vm =await _context.Satisfaction
                    .ProjectTo<GetSatisfactionListDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return vm;
            }
        }
    }
}
