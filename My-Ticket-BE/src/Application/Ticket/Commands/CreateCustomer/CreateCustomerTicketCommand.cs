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

namespace CleanArchitecture.Application.Ticket.Commands.CreateCustomer
{
    public class CreateCustomerTicketCommand : IRequest<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public class CreateCustomerTicketCommandHandler : IRequestHandler<CreateCustomerTicketCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly IEmail _email;
            private readonly ICurrentUserService _currentUserService;
            private readonly IIdentityService _userManager;
            public CreateCustomerTicketCommandHandler(IApplicationDbContext context, IEmail email, ICurrentUserService currentUserService, IIdentityService userManager)
            {
                _context = context;
                _email = email;
                _currentUserService = currentUserService;
                _userManager = userManager;
            }

            public async Task<int> Handle(CreateCustomerTicketCommand request, CancellationToken cancellationToken)
            {
                TicketHeader ticket = null;
                int result = 0;


                ticket = new TicketHeader
                {
                    Title = request.Title,
                    Description = request.Description,
                    Priority = await _context.Priority.FirstOrDefaultAsync(x => x.Name == "Unknow"),
                    Project = await _context.Project.FirstOrDefaultAsync(x => x.Name == "Unknow"),
                    Type = await _context.Type.FirstOrDefaultAsync(x => x.Name == "Unknow"),
                    Status = await _context.Status.FirstOrDefaultAsync(x => x.Name == "Open"),
                    Requester = await _context.User.FirstOrDefaultAsync(x => x.Id == _currentUserService.UserId),
                    AssignTO = null,
                    ClosedDate = null,
                };

                    

                _context.TickerHeader.Add(ticket);
                
                await _context.SaveChangesAsync(cancellationToken);

                ticket.InternTitle = ticket.Title + " - #" + ticket.Id;

                result = await _context.SaveChangesAsync(cancellationToken);
                if (result > 0)
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
