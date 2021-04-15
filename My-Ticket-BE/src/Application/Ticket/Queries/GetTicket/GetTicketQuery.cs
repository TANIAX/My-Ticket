using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Exceptions;
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

namespace CleanArchitecture.Application.Ticket.Queries.GetTicket
{
    public class GetTicketQuery :IRequest<GetTicketDTO>
    {
        public int Id { get; set; }
        public GetTicketQuery(int id)
        {
            this.Id = id;
        }
        
        public class GetTicketQueryHandler : IRequestHandler<GetTicketQuery, GetTicketDTO>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly ICurrentUserService _currentUserService;
            private readonly IIdentityService _userManager;

            public  GetTicketQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService, IIdentityService userManager)
            {
                _context = context;
                _mapper = mapper;
                _currentUserService = currentUserService;
                _userManager = userManager;
            }

            public async Task<GetTicketDTO> Handle(GetTicketQuery request, CancellationToken cancellationToken)
            {
                bool isMemberOrAdmin = false;
                TicketHeader ticket = null;

                if (await _userManager.IsInRoleAsync(_currentUserService.UserId, "Member"))
                    isMemberOrAdmin = true;
                if (await _userManager.IsInRoleAsync(_currentUserService.UserId, "Admin"))
                    isMemberOrAdmin = true;

                var vm = new GetTicketDTO();

                vm = await _context.TickerHeader
                    .ProjectTo<GetTicketDTO>(_mapper.ConfigurationProvider)
                    .Where(x=>x.Id == request.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (!isMemberOrAdmin && vm.Requester?.Id != _currentUserService.UserId)
                {
                    throw new NotFoundException(nameof(TicketHeader), request.Id);
                }
                else
                {
                    //Set readed to ticket
                    ticket = await _context.TickerHeader.FirstOrDefaultAsync(x => x.Id == request.Id);
                    if (isMemberOrAdmin)
                        ticket.Readed = TicketHeader.ReadedByMemberOrAdmin;
                    else
                        ticket.Readed = TicketHeader.ReadedByCustomer;

                    await _context.SaveChangesAsync(cancellationToken);
                }
                    

                return vm;
            }
        }
    }
}
