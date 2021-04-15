using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.SharedKernel.Identity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Ticket.Queries.Wearable.GetUnreadAndOpenNumber
{
    public class UnreadAndOpenQuery : IRequest<UnreadAndOpenDTO>
    {
        public string Email { get; set; }

        public class UnreadAndOpenQueryHandler : IRequestHandler<UnreadAndOpenQuery, UnreadAndOpenDTO>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IIdentityService _userManager;

            public UnreadAndOpenQueryHandler(IApplicationDbContext context, IMapper mapper, IIdentityService userManager)
            {
                _context = context;
                _mapper = mapper;
                _userManager = userManager;
            }

            public async Task<UnreadAndOpenDTO> Handle(UnreadAndOpenQuery request, CancellationToken cancellationToken)
            {
                if (request.Email == null || request.Email == "")
                {
                    throw new NotFoundException(nameof(User), request.Email);
                }

                var member = _context.User.Where(x => x.Email == request.Email).FirstOrDefault();
                var vm = new UnreadAndOpenDTO();

                vm.Open = _context.TickerHeader.Where(x => x.ClosedDate == null).Where(x => x.AssignTO.Id == member.Id).Count();
                vm.Unread = _context.TickerHeader.Where(x => x.Readed == TicketHeader.ReadedByCustomer).Where(x => x.AssignTO.Id == member.Id).Where(x => x.ClosedDate == null).Count();
                vm.PriorityHightOrUrgent = _context.TickerHeader.Where(x => x.Priority.Id >= 4).Where(x => x.AssignTO.Id == member.Id).Where(x => x.ClosedDate == null).Count();

                return vm;
            }
        }

    }
}