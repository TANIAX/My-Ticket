using CleanArchitecture.Application.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Project.Queries.GetProjectList
{
    public class GetProjectListDTO :IMapFrom<CleanArchitecture.Domain.Entities.Project>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
