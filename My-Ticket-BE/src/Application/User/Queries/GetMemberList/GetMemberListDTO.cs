using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.User.Queries.GetMemberList
{
    public class GetMemberListDTO : IMapFrom<CleanArchitecture.Domain.Entities.AppUser>
    {
        public string Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
    }
}
