using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Application.Common.Mappings;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CleanArchitecture.Application.Ticket.Queries.Create
{
    public class TypeDTO : IMapFrom<CleanArchitecture.Domain.Entities.Type>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
