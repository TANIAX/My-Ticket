using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.User.Queries.Employee.List
{
    public class MyEmployeesDTO : IMapFrom<AppUser>
    {
        public string Id { get; set; }
        public ICollection<EmployeeListDTO> EmployeeList { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AppUser, MyEmployeesDTO>()
                .ForMember(x => x.EmployeeList, opt => opt.MapFrom(y => y.UserList));
        }
    }
}
