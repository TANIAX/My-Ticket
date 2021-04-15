using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Group.Commands.Update
{
    public class UpdateGroupCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, int>
        {
            private readonly IApplicationDbContext _context;

            public UpdateGroupCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
            {
               Domain.Entities.Group group;

               group = await _context.Group.FirstOrDefaultAsync(x => x.Id == request.Id);
               
               if(group == null)
                   throw new NotFoundException(nameof(Domain.Entities.Group),request.Id);

               group.Name = request.Name;

               return await _context.SaveChangesAsync(cancellationToken);

            }
        }
    }
}

