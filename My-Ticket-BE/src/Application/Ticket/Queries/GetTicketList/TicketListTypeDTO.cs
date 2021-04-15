using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Ticket.Queries.GetTicketList
{
    public class TicketListTypeDTO : IMapFrom<CleanArchitecture.Domain.Entities.Type>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
