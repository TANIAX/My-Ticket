using CleanArchitecture.Application.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Status.Queries.GetStatusList
{
    public class GetStatusListDTO :IMapFrom<CleanArchitecture.Domain.Entities.Status>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
