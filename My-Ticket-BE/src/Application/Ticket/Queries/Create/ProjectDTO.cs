using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Ticket.Queries.Create
{
    public class ProjectDTO : IMapFrom<CleanArchitecture.Domain.Entities.Project>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
