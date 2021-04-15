using CleanArchitecture.Application.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.User.Queries.GetCurrentUser
{
    public class GroupDTO : IMapFrom<CleanArchitecture.Domain.Entities.Group>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
