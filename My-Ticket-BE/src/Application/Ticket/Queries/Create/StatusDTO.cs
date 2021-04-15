using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Ticket.Queries.Create
{
    public class StatusDTO : IMapFrom<Domain.Entities.Status>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
