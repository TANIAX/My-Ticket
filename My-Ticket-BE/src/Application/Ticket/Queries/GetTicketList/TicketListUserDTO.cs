using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Ticket.Queries.GetTicketList
{
    public class TicketListUserDTO : IMapFrom<AppUser>
    {
        public string Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string PP { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<AppUser, TicketListUserDTO>()
                .ForMember(d => d.PP, opt => opt.MapFrom(s => s.ProfilPicture));
        }
    }
}
