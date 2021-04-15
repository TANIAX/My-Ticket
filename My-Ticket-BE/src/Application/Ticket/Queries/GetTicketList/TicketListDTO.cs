using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Ticket.Queries.GetTicketList
{
    public class TicketListDTO : IMapFrom<TicketHeader>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime? LastResponseDate { get; set; }
        public string LastResponseByEmail { get; set; }
        public string Email { get; set; }
        public int Readed { get; set; }

        public TicketListUserDTO LastResponseByUser { get; set; }
        public TicketListUserDTO Requester { get; set; }
        public TicketListUserDTO AssignTO { get; set; }
        public TicketListPriorityDTO Priority { get; set; }
        public TicketListGroupDTO Group { get; set; }
        public TicketListStatusDTO Status { get; set; }
        public TicketListTypeDTO Type { get; set; }
        public TicketListProjectDTO Project { get; set; }

        public TicketListDTO()
        {
            this.AssignTO = new TicketListUserDTO();
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TicketHeader, TicketListDTO>()
                .ForMember(d => d.CreationDate, opt => opt.MapFrom(s => s.Created));
        }
    }
}
