using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.User.Queries.EditMyProfile
{
    public class EditMyProfileDTO : IMapFrom<AppUser>
    {
        public string Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string PP { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsCompany{ get; set; }
        public string CompanyName { get; set; }
        public bool IsEmployee { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string Locality { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public DateTime Birthdate { get; set; }
        public int NumberOfEmployee { get; set; }
        public ICollection<EditMyProfileStoredReplyDTO> StoredReplies { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AppUser, EditMyProfileDTO>()
                .ForMember(x => x.PP, opt => opt.MapFrom(y => y.ProfilPicture))
                .ForMember(x => x.NumberOfEmployee, opt => opt.MapFrom(y => y.UserList.Count))
                .ForMember(x => x.Birthdate, opt => opt.MapFrom(y => y.BirthDate))
                .ForMember(x => x.StoredReplies, opt => opt.MapFrom(y => y.StoredReply));
        }
    }
}
