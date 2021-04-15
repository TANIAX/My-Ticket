using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.User.Queries.Employee.List;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Group.Queries
{
    public class GroupListQuery : IRequest<List<GroupDTO>>
    {
        public class GroupListQueryHandler : IRequestHandler<GroupListQuery, List<GroupDTO>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GroupListQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<GroupDTO>> Handle(GroupListQuery request, CancellationToken cancellationToken)
            {
                //TODO Convert to automapper when we find a solution for mapping number of member in each group
                var gl = (from g in _context.Group
                    select new GroupDTO()
                    {
                        Id = g.Id,
                        Name = g.Name,
                        NumberOfMember = (from u in _context.User
                            where u.Group.Id == g.Id
                            select u).Count()
                    }).Distinct().ToList();

                return gl;
            }
        }
    }
}
