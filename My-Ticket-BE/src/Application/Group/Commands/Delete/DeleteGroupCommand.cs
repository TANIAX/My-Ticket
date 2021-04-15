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

namespace CleanArchitecture.Application.Group.Commands.Delete
{
    public class DeleteGroupCommand : IRequest<int>
    {
        public int Id { get; set; }
        public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, int>
        {
            private readonly IApplicationDbContext _context;

            public DeleteGroupCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
            {
               Domain.Entities.Group group;

               group = await _context.Group.FirstOrDefaultAsync(x => x.Id == request.Id);
               
               if(group == null)
                   throw new NotFoundException(nameof(Domain.Entities.Group),request.Id);

               _context.Group.Remove(group);

               return await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}

