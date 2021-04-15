using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Ticket.Queries.GetTicket
{
    public class TicketLineDTO : IMapFrom<TicketLine>
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool AskForClose { get; set; }
        public TicketUserDTO ResponseBy { get; set; }
        public DateTime ResponseDate { get; set; }
        public string Email { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TicketLine, TicketLineDTO>()
                .ForMember(d => d.ResponseDate, opt => opt.MapFrom(s => s.Created));
        }
    }
}
