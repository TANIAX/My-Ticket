using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.User.Queries.GetCurrentUser
{
    public class GetCurrentUserDTO : IMapFrom<CleanArchitecture.Domain.Entities.AppUser>
    {
        public GetCurrentUserDTO()
        {
            StoredReply = new HashSet<StoredReplyDTO>();
            UserList = new HashSet<UserDTO>();
        }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public int? ZipCode { get; set; }
        public string Locality { get; set; }
        public string Street { get; set; }
        public string PP { get; set; }
        public bool IsCompany { get; set; }
        public string? CompanyName { get; set; }
        public bool IsEmployee { get; set; }
        public string Signature { get; set; }
        public string Language { get; set; }
        public virtual GroupDTO Group { get; set; }
        public virtual ICollection<StoredReplyDTO> StoredReply { get; private set; }
        public virtual ICollection<UserDTO> UserList { get; private set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CleanArchitecture.Domain.Entities.AppUser, GetCurrentUserDTO>()
                .ForMember(d => d.PP, opt => opt.MapFrom(s => s.ProfilPicture));
        }
    }
}
