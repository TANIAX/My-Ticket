using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.SharedKernel.Identity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Ticket.Commands.CreateReplyTicketByMail
{
    public class CreateReplyTicketByMailCommand : IRequest<int>
    {
        public class CreateReplyTicketByMailCommandHandler : IRequestHandler<CreateReplyTicketByMailCommand,int>
        {
            IApplicationDbContext _context;
            IEmail _email;
            IIdentityService _userManager;
            ICloudinary _cloudinary;
            public CreateReplyTicketByMailCommandHandler(IApplicationDbContext context, IEmail email, IIdentityService userManager, ICloudinary cloudinary)
            {
                _context = context;
                _email = email;
                _userManager = userManager;
                _cloudinary = cloudinary;
            }

            public async Task<int> Handle(CreateReplyTicketByMailCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    List<Email> lt = _email.ReadInbox() as List<Email>;
                    string senderEmail = "";
                    bool createTicket = true;
                    string internRef = "";
                    int ticketId = 0;
                    int index = -1;
                    string subject = "";
                    bool isMemberOrAdmin = false;
                    List<byte[]> attachementList;
                    TicketLine ticketLine;
                    TicketHeader ticket;
                    AppUser user;
                    int result = 0;


                    foreach (var t in lt)
                    {
                        user = _context.User.FirstOrDefault(x => x.Email.ToUpper() == t.from.ToUpper());
                        if (user == null)
                        {
                            senderEmail = t.from;
                        }
                        else
                        {
                            if (await _userManager.IsInRoleAsync(user.Id, "Member"))
                                isMemberOrAdmin = true;
                            else if (await _userManager.IsInRoleAsync(user.Id, "Admin"))
                                isMemberOrAdmin = true;
                        }
                        if (t.Subject.Length > 0)
                        {
                            subject = Regex.Replace(t.Subject, @"\s+", "");
                            index = subject.IndexOf("-#");
                            if (index > -1)
                            {
                                internRef = subject.Substring(index + 2);
                                Int32.TryParse(internRef, out ticketId);
                                ticket = _context.TickerHeader.FirstOrDefault(x => x.Id == ticketId);
                                if (ticket != null)
                                {
                                    if (user == null && ticket.Email.ToUpper() == senderEmail.ToUpper())
                                    {
                                        ticket.Status = _context.Status.FirstOrDefault(x => x.Name == "Open");
                                        ticketLine = new TicketLine
                                        {
                                            AskForClose = false,
                                            Content = t.Body,
                                            Email = senderEmail,
                                        };
                                    }
                                    else if ((user != null && ticket.Email.ToUpper() == user.Email.ToUpper()) || isMemberOrAdmin)
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
                                            
                                        ticketLine = new TicketLine
                                        {
                                            AskForClose = false,
                                            Content = t.Body,
                                            ResponseBy = user,
                                        };
                                    }
                                    else
                                    {
                                        throw new NotFoundException(nameof(TicketHeader), ticket.Id);
                                    }

                                    foreach (var str in _cloudinary.UploadToCloudinary(t.attachement.ToList()))
                                    {
                                        ticketLine.Content += str;
                                    }
                                    ticket.TicketLine.Add(ticketLine);
                                    result += await _context.SaveChangesAsync(cancellationToken);
                                }
                                else
                                {
                                    throw new NotFoundException(nameof(TicketHeader), ticketId);
                                }
                            }
                            else
                            {
                                //We need to create the ticket
                                ticket = new TicketHeader
                                {
                                    Title = t.Subject,
                                    Description = t.Body,
                                    Priority = _context.Priority.FirstOrDefault(x => x.Name == "Unknow"),
                                    Project = _context.Project.FirstOrDefault(x => x.Name == "Unknow"),
                                    Type = _context.Type.FirstOrDefault(x=>x.Name == "Unknow"),
                                    AssignTO = null,
                                    ClosedDate = null,
                                    Requester = user,
                                    Status = _context.Status.FirstOrDefault(x => x.Name == "Open"),
                                    CreatedBy = user?.Id,
                                    Email = user?.Email,
                                    Readed = TicketHeader.ReadedByCustomer  
                                };
                                if (user == null)
                                {
                                    ticket.Email = senderEmail;
                                }
                                foreach (var str in _cloudinary.UploadToCloudinary(t.attachement.ToList()))
                                {
                                    ticket.Description += str;
                                }

                                _context.TickerHeader.Add(ticket);
                                await _context.SaveChangesAsync(cancellationToken);
                                ticket.InternTitle = ticket.Title + " - #" + ticket.Id;

                                result += await _context.SaveChangesAsync(cancellationToken);
                                if (result > 0 && !isMemberOrAdmin)
                                {
                                    if (user != null)
                                    {
                                        senderEmail = user.Email;
                                    }

                                    _email.SendEmail(senderEmail, ticket.InternTitle, "Your ticket is successfully created. <br>" +
                                                                                                        "We will reply soon as possible.<br>" +
                                                                                                        "<hr>Ticket content :<br>" + ticket.Description);
                                }
                            }
                        }

                        else
                        {
                            throw new ApplicationException();
                        }
                    }

                    return result;
                    
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
        }
    }
}
