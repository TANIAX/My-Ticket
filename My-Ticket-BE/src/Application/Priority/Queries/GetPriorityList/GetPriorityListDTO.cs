using CleanArchitecture.Application.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Priority.Queries.GetPriorityList
{
    public class GetPriorityListDTO : IMapFrom<CleanArchitecture.Domain.Entities.Priority>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
