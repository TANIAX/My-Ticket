using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.User.Queries.MyAccount
{
    public class MyAccountDTO : IMapFrom<AppUser>
    {
        public string Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string PP { get; set; }
        public string PhoneNumber { get; set; }
        public string Language { get; set; }
        public string Signature { get; set; }
        public bool IsCompany { get; set; }
        public bool IsEmployee { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AppUser, MyAccountDTO>()
                .ForMember(x => x.PP, opt => opt.MapFrom(y => y.ProfilPicture));
        }
    }
}
