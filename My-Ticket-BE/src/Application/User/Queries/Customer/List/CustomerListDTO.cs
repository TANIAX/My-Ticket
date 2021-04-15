using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.User.Queries.Employee.List;
using CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.User.Queries.Customer.List
{
    public class CustomerListDTO : IMapFrom<AppUser>
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string Locality { get; set; }
        public int ZipCode { get; set; }
        public string Street { get; set; }

        public ICollection<EmployeeListDTO> UserList { get; set; }

    }
}
