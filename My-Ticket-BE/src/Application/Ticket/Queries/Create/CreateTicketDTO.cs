using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Ticket.Queries.Create
{
    public class CreateTicketDTO 
    {
        public ICollection<StatusDTO> Status { get; set; }
        public ICollection<TypeDTO> Types { get; set; }
        public ICollection<PriorityDTO> Priorities { get; set; }
        public ICollection<ProjectDTO> Projects { get; set; }
        public ICollection<GroupDTO> Groups { get; set; }
        public ICollection<MemberDTO> Members { get; set; }
        public ICollection<CustomerDTO> Customers { get; set; }
    }
}
