using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CleanArchitecture.Application.Ticket.Queries.Create
{
    public class PriorityDTO : IMapFrom<CleanArchitecture.Domain.Entities.Priority>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
