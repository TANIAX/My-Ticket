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

namespace CleanArchitecture.Application.Ticket.Commands.Close
{
    public class CloseTicketsCommand : IRequest<int>
    {
        public List<TicketHeader> TicketList { get; set; }
        public class CloseTicketsCommandHandler : IRequestHandler<CloseTicketsCommand, int>
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

            public async Task<int> Handle(CloseTicketsCommand request, CancellationToken cancellationToken)
            {
                TicketHeader ticket = null;
                AppUser user = null;
                bool isMemberOrAdmin = false;
                string userEmail;
                string messageMail = "Your ticket is now closed.<br>You can reply to this ticket to reopen it.";

                user = await _context.User.FirstOrDefaultAsync(x => x.Id == _currentUserService.UserId);

                if (await _userManager.IsInRoleAsync(user.Id, "Admin") || await _userManager.IsInRoleAsync(user.Id, "Member"))
                    isMemberOrAdmin = true;

                foreach (var model in request.TicketList)
                {
                    ticket = await _context.TickerHeader.Include(x=>x.Status).FirstOrDefaultAsync(x => x.Id == model.Id);
                    if (ticket != null)
                    {
                        if (isMemberOrAdmin || (ticket.Requester != null && ticket.Requester.Id == _currentUserService.UserId))
                        {
                            ticket.ClosedDate = DateTime.Now;
                            ticket.Status = await _context.Status.FirstOrDefaultAsync(x => x.Name == "Closed");
                        }

                        //Send mail to announce the ticket is closed
                        if (isMemberOrAdmin)
                        {
                            if (ticket.Requester != null)
                            {
                                userEmail = ticket.Requester?.Email;
                            }
                            else
                            {
                                userEmail = ticket.Email;
                            }
                            _email.SendEmail(userEmail, ticket.InternTitle, messageMail);
                        }

                        
                    }
                    ticket = null;
                    userEmail = "";
                }
                return await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
