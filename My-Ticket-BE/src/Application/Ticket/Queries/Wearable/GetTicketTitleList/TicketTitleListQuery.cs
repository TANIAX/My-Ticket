using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.SharedKernel.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Ticket.Queries.Wearable.GetTicketTitleList
{
    public class TicketTitleListQuery : IRequest<List<TicketTitleListDTO>>
    {
        public int Type { get; set; }
        public class TicketTitleListQueryHandler : IRequestHandler<TicketTitleListQuery, List<TicketTitleListDTO>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IIdentityService _userManager;
            private readonly ICurrentUserService _currentUserService;

            public TicketTitleListQueryHandler(IApplicationDbContext context, IMapper mapper, IIdentityService userManager, ICurrentUserService currentUserService)
            {
                _context = context;
                _mapper = mapper;
                _userManager = userManager;
                _currentUserService = currentUserService;
            }

            public async Task<List<TicketTitleListDTO>> Handle(TicketTitleListQuery request, CancellationToken cancellationToken)
            {

                if (request.Type == 1)
                {
                    return await _context.TickerHeader
                     .Where(x => x.AssignTO.Id == _currentUserService.UserId)
                     .Where(x => x.Readed == TicketHeader.ReadedByCustomer)
                     .Where(x => x.ClosedDate == null)
                     .OrderBy(x => x.Created)
                     .ProjectTo<TicketTitleListDTO>(_mapper.ConfigurationProvider)
                     .ToListAsync(cancellationToken);
                }
                else
                {
                    return await _context.TickerHeader
                     .Where(x => x.AssignTO.Id == _currentUserService.UserId)
                     .Where(x => x.ClosedDate == null)
                     .OrderBy(x => x.Created)
                     .ProjectTo<TicketTitleListDTO>(_mapper.ConfigurationProvider)
                     .ToListAsync(cancellationToken);
                }
                

            }
        }
    }
}
