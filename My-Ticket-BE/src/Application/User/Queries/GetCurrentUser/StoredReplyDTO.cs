using CleanArchitecture.Application.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.User.Queries.GetCurrentUser
{
    public class StoredReplyDTO : IMapFrom<CleanArchitecture.Domain.Entities.StoredReply>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Reply { get; set; }
    }
}
