using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.User.Queries.EditMyProfile;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.User.Queries.MyAccount.EditMyProfile
{
    public class EditMyProfileQuery : IRequest<EditMyProfileDTO>
    {
        public class GetTodosQueryHandler : IRequestHandler<EditMyProfileQuery, EditMyProfileDTO>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly ICurrentUserService _currentUserService;

            public GetTodosQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
            {
                _context = context;
                _mapper = mapper;
                _currentUserService = currentUserService;
            }

            public async Task<EditMyProfileDTO> Handle(EditMyProfileQuery request, CancellationToken cancellationToken)
            {
                var vm = new EditMyProfileDTO();

                vm = await _context.User
                    .Where(x => x.Id == _currentUserService.UserId)
                .ProjectTo<EditMyProfileDTO>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

                return vm;
            }
        }
    }
}
