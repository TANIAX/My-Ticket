using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.User.Queries.Employee.List;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.SharedKernel.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Ticket.Queries.Create
{
    public class CreateTicketQuery : IRequest<CreateTicketDTO>
    {
        public class CreateTicketQueryHandler : IRequestHandler<CreateTicketQuery, CreateTicketDTO>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IIdentityService _userManager;
            public CreateTicketQueryHandler(IApplicationDbContext context, IMapper mapper, IIdentityService userManager)
            {
                _context = context;
                _mapper = mapper;
                _userManager = userManager;
            }

            public async Task<CreateTicketDTO> Handle(CreateTicketQuery request, CancellationToken cancellationToken)
            {
                var memberList = await _userManager.UsersInRoleAsync("Member") as List<ApplicationUser>;
                var CustomerList = await _userManager.UsersInRoleAsync("Customer") as List<ApplicationUser>;
                List<CustomerDTO> appCustomerList = new List<CustomerDTO>();
                List<MemberDTO> appMemberList = new List<MemberDTO>();
                foreach (var member in memberList)
                {
                    appMemberList.Add(_context.User.Where(x => x.Id == member.Id).ProjectTo<MemberDTO>(_mapper.ConfigurationProvider).FirstOrDefault());
                }
                foreach (var customer in CustomerList)
                {
                    appCustomerList.Add(_context.User.Where(x => x.Id == customer.Id).ProjectTo<CustomerDTO>(_mapper.ConfigurationProvider).FirstOrDefault());
                }

                var vm = new CreateTicketDTO
                {
                    Priorities = await _context.Priority.ProjectTo<PriorityDTO>(_mapper.ConfigurationProvider).ToListAsync(),
                    Projects = await _context.Project.ProjectTo<ProjectDTO>(_mapper.ConfigurationProvider).ToListAsync(),
                    Status = await _context.Status.ProjectTo<StatusDTO>(_mapper.ConfigurationProvider).ToListAsync(),
                    Types = await _context.Type.ProjectTo<TypeDTO>(_mapper.ConfigurationProvider).ToListAsync(),
                    Groups = await _context.Group.ProjectTo<GroupDTO>(_mapper.ConfigurationProvider).ToListAsync(),
                    Members = appMemberList,
                    Customers = appCustomerList
                };
                
                return vm;
            }
        }
    }
}
