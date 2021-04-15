using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.User.Queries.EditMyProfile
{
    public class EditMyProfileStoredReplyDTO : IMapFrom<CleanArchitecture.Domain.Entities.StoredReply>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Reply { get; set; }
    }
}
