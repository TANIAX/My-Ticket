using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.SharedKernel.GlobalVar;
using CleanArchitecture.SharedKernel.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Ticket.Commands.Reply
{
    public class ReplyTicketCommand : IRequest<int>
    {
        public int TicketHeaderId { get; set; }
        public bool AskForClose { get; set; }
        public string Response { get; set; }

        public class ReplyTicketCommandHandler : IRequestHandler<ReplyTicketCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            private readonly IIdentityService _userManager;
            private readonly IEmail _email;
            public ReplyTicketCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IIdentityService userManager, IEmail email)
            {
                _context = context;
                _currentUserService = currentUserService;
                _userManager = userManager;
                _email = email;
            }

            public async Task<int> Handle(ReplyTicketCommand request, CancellationToken cancellationToken)
            {
                bool isMemberOrAdmin = false;
                TicketHeader ticket = null;
                TicketLine ticketLine = null;
                AppUser user = null;
                string sendToemail = "";
                int result = 0;
                string messageMail = "Your ticket is now closed.<br>You can reply to this ticket to reopen it.";

                user = await _context.User.FirstOrDefaultAsync(x => x.Id == _currentUserService.UserId);

                if (await _userManager.IsInRoleAsync(_currentUserService.UserId, "Admin") || await _userManager.IsInRoleAsync(_currentUserService.UserId, "Member"))
                    isMemberOrAdmin = true;

                ticket = await _context.TickerHeader.Include(x=>x.Requester).FirstOrDefaultAsync(x => x.Id == request.TicketHeaderId);
                ticketLine = new TicketLine();

                if (ticket == null || (ticket.Requester == null && !isMemberOrAdmin) || (!isMemberOrAdmin && !ticket.Requester.Equals(user)))
                    throw new NotFoundException(nameof(TicketHeader), request.TicketHeaderId);

                if (ticket.Requester == null)
                    sendToemail = ticket.Email;
                else
                    sendToemail = ticket.Requester.Email;

                if (!request.AskForClose)
                {
                    if (isMemberOrAdmin)
                    {
                        ticket.Status = _context.Status.FirstOrDefault(x => x.Name == "Waiting on customer");
                        ticket.Readed = TicketHeader.ReadedByMemberOrAdmin;
                    }
                    else
                    {
                        ticket.Status = _context.Status.FirstOrDefault(x => x.Name == "Open");
                        ticket.Readed = TicketHeader.ReadedByCustomer;
                    }
                    ticketLine.Content = request.Response;
                    ticketLine.ResponseBy = user;
                    ticketLine.Email = user.Email;

                    ticket.TicketLine.Add(ticketLine);
                }
                else
                {
                    ticket.ClosedDate = DateTime.Now;
                    ticket.Status = _context.Status.FirstOrDefault(x => x.Name == "Closed");
                    if(isMemberOrAdmin)
                        _email.SendEmail(sendToemail, ticket.InternTitle, messageMail);
                }

                result = await _context.SaveChangesAsync(cancellationToken);

                if (result > 0)
                {
                   _email.SendEmail(sendToemail, ticket.InternTitle, "A new response is available on your ticket, click " +
                                                                                        "<a href ='" + GlobalVar.FEUrl + "Ticket/" + ticket.Id + "' >here.</a>" +
                                                                                        "to see.");
                }

                return result;
            }
        }
    }
}
