using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.SharedKernel.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Ticket.Commands.Delete
{
    public class DeleteTicketsCommand : IRequest<Unit>
    {
        public string TicketList { get; set; }
        public string Reason { get; set; }
        public class CloseTicketsCommandHandler : IRequestHandler<DeleteTicketsCommand, Unit>
        {
            IApplicationDbContext _context;
            ICurrentUserService _currentUserService;
            IIdentityService _userManager;
            IEmail _email;
            public CloseTicketsCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IIdentityService userManager, IEmail email)
            {
                _context = context;
                _currentUserService = currentUserService;
                _userManager = userManager;
                _email = email;
            }

            public async Task<Unit> Handle(DeleteTicketsCommand request, CancellationToken cancellationToken)
            {
                TicketHeader ticket = null;
                AppUser user = null;
                int id = 0;
                string[] idsString;
                bool isMemberOrAdmin = false;
                string sendToEmail;
                string messageMail = "Your ticket is delete.<br>Reason : " + request.Reason; 

                user = await _context.User.FirstOrDefaultAsync(x => x.Id == _currentUserService.UserId);
                idsString = request.TicketList.Split(",");
                if (await _userManager.IsInRoleAsync(user.Id, "Admin") || await _userManager.IsInRoleAsync(user.Id, "Member"))
                    isMemberOrAdmin = true;

                foreach (var idString in idsString)
                {
                    Int32.TryParse(idString, out id);
                    if (id != 0)
                    {
                        ticket = await _context.TickerHeader.FirstOrDefaultAsync(x => x.Id == id);

                        if (ticket == null)
                        {
                            throw new NotFoundException(nameof(TicketHeader), id);
                        }    
                        else
                        {
                            if (isMemberOrAdmin)
                            {
                                if (ticket.Requester != null)
                                    sendToEmail = ticket.Requester.Email;
                                else
                                    sendToEmail = ticket.Email;

                                _email.SendEmail(sendToEmail, ticket.InternTitle, messageMail);
                                sendToEmail = "";
                            }
                            if (isMemberOrAdmin || (ticket.Requester != null && ticket.Requester.Id == _currentUserService.UserId))
                            {
                                _context.TickerHeader.Remove(ticket);
                            }
                        }                   
                    }
                    else
                    {
                        throw new NotFoundException(nameof(CleanArchitecture.Domain.Entities.StoredReply), id);
                    }
                    id = 0;
                }

                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
