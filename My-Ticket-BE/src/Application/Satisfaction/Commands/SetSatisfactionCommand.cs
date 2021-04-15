using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Satisfaction.Commands
{
    public class SetSatisfactionCommand : IRequest<int>
    {
        public int TicketId { get; set; }
        public int SatisfactionId { get; set; }

        public class SetSatisfactionCommandHandler : IRequestHandler<SetSatisfactionCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public SetSatisfactionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }
            public async Task<int> Handle(SetSatisfactionCommand request, CancellationToken cancellationToken)
            {
                TicketHeader t;
                CleanArchitecture.Domain.Entities.Satisfaction s;

                t = await _context.TickerHeader.FirstOrDefaultAsync(x => x.Id == request.TicketId);
                s = await _context.Satisfaction.FirstOrDefaultAsync(x => x.Id == request.SatisfactionId);

                if(t == null)
                {
                    throw new NotFoundException(nameof(CleanArchitecture.Domain.Entities.TicketHeader), request.TicketId);
                }
                if (s == null)
                {
                    throw new NotFoundException(nameof(CleanArchitecture.Domain.Entities.Satisfaction), request.SatisfactionId);
                }
                if (t.Requester != null && t.Requester?.Id != _currentUserService.UserId)
                {
                    throw new NotFoundException(nameof(CleanArchitecture.Domain.Entities.TicketHeader), request.TicketId);
                }

                t.Satisfaction = s;

                return await _context.SaveChangesAsync(cancellationToken);

            }
        }
    }
}
