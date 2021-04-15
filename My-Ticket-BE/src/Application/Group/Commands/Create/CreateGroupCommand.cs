using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.Group.Commands.Create
{
    public class CreateGroupCommand : IRequest<int>
    {
        public string Name { get; set; }
        public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, int>
        {
            private readonly IApplicationDbContext _context;

            public CreateGroupCommandHandler (IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
            {
               Domain.Entities.Group group;

               group = new Domain.Entities.Group
               {
                   Name = request.Name
               };

               _context.Group.Add(group);

               await _context.SaveChangesAsync(cancellationToken);

               return group.Id;

            }
        }
    }
}

