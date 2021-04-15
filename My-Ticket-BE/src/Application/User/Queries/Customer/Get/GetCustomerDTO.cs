using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.User.Queries.Customer.Get
{
    public class GetCustomerDTO : IMapFrom<CleanArchitecture.Domain.Entities.AppUser>
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public int? ZipCode { get; set; }
        public string Locality { get; set; }
        public string Street { get; set; }
        public string? CompanyName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CleanArchitecture.Domain.Entities.AppUser, GetCustomerDTO>();
        }
    }
}
