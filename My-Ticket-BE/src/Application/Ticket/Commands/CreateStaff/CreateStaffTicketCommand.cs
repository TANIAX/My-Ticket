using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.SharedKernel.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Ticket.Commands.CreateStaff
{
    public class CreateStaffTicketCommand : IRequest<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool CloseImmediatly { get; set; }
        public int Project { get; set; }
        public int Type { get; set; }
        public int Priority { get; set; }
        public string AssignTo { get; set; }
        public string Requester { get; set; }
        public int Group { get; set; }

        public class CreateStaffTicketCommandHandler : IRequestHandler<CreateStaffTicketCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly IEmail _email;
            private readonly ICurrentUserService _currentUserService;
            private readonly IIdentityService _userManager;
            public CreateStaffTicketCommandHandler(IApplicationDbContext context, IEmail email, ICurrentUserService currentUserService, IIdentityService userManager)
            {
                _context = context;
                _email = email;
                _currentUserService = currentUserService;
                _userManager = userManager;
            }

            public async Task<int> Handle(CreateStaffTicketCommand request, CancellationToken cancellationToken)
            {
                AppUser user = null;
                TicketHeader ticket = null;
                int result = 0;

                ticket = new TicketHeader
                {
                    Title = request.Title,
                    Description = request.Description,
                    Priority = await _context.Priority.FirstOrDefaultAsync(x => x.Id == request.Priority),
                    Project = await _context.Project.FirstOrDefaultAsync(x => x.Id == request.Project),
                    Type = await _context.Type.FirstOrDefaultAsync(x => x.Id == request.Type),
                    Group = await _context.Group.FirstOrDefaultAsync(x => x.Id == request.Group),
                    AssignTO = await _context.User.FirstOrDefaultAsync(x=>x.Id == request.AssignTo),
                    Requester = await _context.User.FirstOrDefaultAsync(x => x.Id == request.Requester),
                    ClosedDate = null,
                };
                if (request.CloseImmediatly == true)
                    ticket.Status = await _context.Status.FirstOrDefaultAsync(x => x.Name == "Closed");
                else
                    ticket.Status = await _context.Status.FirstOrDefaultAsync(x => x.Name == "Open");

                _context.TickerHeader.Add(ticket);
                
                result = await _context.SaveChangesAsync(cancellationToken);

                ticket.InternTitle = ticket.Title + " - #" + ticket.Id;

                result = await _context.SaveChangesAsync(cancellationToken);

                if (result > 0 && ticket.Requester != null)
                {
                    _email.SendEmail(ticket.Requester.Email, ticket.InternTitle, "Your ticket is successfully created. <br>" +
                                                                                        "We will reply soon as possible.<br>" +
                                                                                        "<hr>Ticket content :<br>" + ticket.Description);
                }

                return ticket.Id;
            }
        }
    }
}
