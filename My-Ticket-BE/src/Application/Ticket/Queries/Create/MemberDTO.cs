using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Ticket.Queries.Create
{
    public class MemberDTO : IMapFrom<AppUser>
    {
        public string Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}
