using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Ticket.Commands.Update
{
    public class UpdateTicketCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string Email { get; set; }
        public CleanArchitecture.Domain.Entities.AppUser AssignTO { get; set; }
        public CleanArchitecture.Domain.Entities.AppUser Requester { get; set; }
        public CleanArchitecture.Domain.Entities.Group Group { get; set; }
        public CleanArchitecture.Domain.Entities.Status Status { get; set; }
        public CleanArchitecture.Domain.Entities.Priority Priority { get; set; }
        public CleanArchitecture.Domain.Entities.Project Project { get; set; }
        public CleanArchitecture.Domain.Entities.Type Type { get; set; }
        public CleanArchitecture.Domain.Entities.Satisfaction Satisfaction { get; set; }
        public class UpdateTicketCommandHandler : IRequestHandler<UpdateTicketCommand, int>
        {
            IApplicationDbContext _context;
            IEmail _email;
            public UpdateTicketCommandHandler(IApplicationDbContext context, IEmail email)
            {
                _context = context;
                _email = email;
            }

            public async Task<int> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
            {
                TicketHeader ticket = null;
                CleanArchitecture.Domain.Entities.Status status = null;
                bool close = false;
                string userEmail;
                string messageMail = "Your ticket is now closed.<br>You can reply to this ticket to reopen it.";

                ticket = await _context.TickerHeader
                    .Include(x => x.AssignTO)
                    .Include(x=>x.Group)
                    .Include(x=>x.Priority)
                    .Include(x=>x.Project)
                    .Include(x=>x.Requester)
                    .Include(x=>x.Satisfaction)
                    .Include(x=>x.Status)
                    .Include(x=>x.Type)
                    .Include(x=>x.TicketLine)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                if (ticket == null)
                    throw new NotFoundException(nameof(TicketHeader), request.Id);

                ticket.Description = request.Description;
                ticket.Email = request.Email;
                ticket.Title = request.Title;

                if (request.AssignTO.Id != "")
                    ticket.AssignTO = await _context.User.FirstOrDefaultAsync(x => x.Id == request.AssignTO.Id);

                if (request.Group != null && request.Group.Id != 0)
                    ticket.Group = await _context.Group.FirstOrDefaultAsync(x => x.Id == request.Group.Id);

                if (request.Priority != null && request.Priority.Id != 0)
                    ticket.Priority = await _context.Priority.FirstOrDefaultAsync(x => x.Id == request.Priority.Id);

                if (request.Satisfaction != null && request.Satisfaction.Id != 0)
                    ticket.Satisfaction = await _context.Satisfaction.FirstOrDefaultAsync(x => x.Id == request.Satisfaction.Id);

                if (request.Project != null && request.Project.Id != 0)
                    ticket.Project = await _context.Project.FirstOrDefaultAsync(x => x.Id == request.Project.Id);

                if (request.Status != null && request.Status.Id != 0 && request.Status.Id != ticket.Status.Id)
                {
                    status = await _context.Status.FirstOrDefaultAsync(x => x.Id == request.Status.Id);
                    ticket.Status = await _context.Status.FirstOrDefaultAsync(x => x.Id == request.Status.Id);
                    if (status.Name == "Closed")
                    {
                        ticket.ClosedDate = DateTime.Now;
                        close = true;
                    }
                    else
                    {
                        ticket.ClosedDate = null;
                    }
                }

                if (close)
                {
                    if(ticket.Requester != null)
                    {
                        userEmail = ticket.Requester?.Email;
                    }
                    else
                    {
                        userEmail = ticket.Email;
                    }
                    _email.SendEmail(userEmail, ticket.InternTitle, messageMail);
                }
                return await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
