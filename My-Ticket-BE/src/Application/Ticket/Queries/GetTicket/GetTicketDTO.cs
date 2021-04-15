using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Ticket.Queries.GetTicket
{
    public class GetTicketDTO :IMapFrom<TicketHeader>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Readed { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public Domain.Entities.Status Status { get; set; }
        public CleanArchitecture.Domain.Entities.Priority Priority { get; set; }
        public TicketUserDTO AssignTO { get; set; }
        public TicketUserDTO Requester { get; set; }
        public CleanArchitecture.Domain.Entities.Project Project { get; set; }
        public CleanArchitecture.Domain.Entities.Type Type { get; set; }
        public ICollection<TicketLineDTO> TicketLines { get; set; }
        public string Email { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TicketHeader, GetTicketDTO>()
                .ForMember(d => d.CreationDate, opt => opt.MapFrom(s => s.Created))
                .ForMember(d => d.TicketLines, opt => opt.MapFrom(s => s.TicketLine));
        }
    }
}
