using CleanArchitecture.Application.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Type.Queries.GetTypeList
{
    public class GetTypeListDTO : IMapFrom<CleanArchitecture.Domain.Entities.Type>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
