using CleanArchitecture.Application.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Satisfaction.Queries.GetSatisfactionList
{
    public class GetSatisfactionListDTO : IMapFrom<CleanArchitecture.Domain.Entities.Satisfaction>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
