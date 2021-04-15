using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.StoredReply.Commands.Delete
{
    public class DeleteStoredReplyCommand : IRequest<int>
    {
        public string storedReplyList { get; set; }
        public class DeleteStoredReplyCommandHandler : IRequestHandler<DeleteStoredReplyCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public DeleteStoredReplyCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(DeleteStoredReplyCommand request, CancellationToken cancellationToken)
            {
                CleanArchitecture.Domain.Entities.StoredReply sr;
                AppUser user;
                int id = 0;
                string[] idsString;

                user = _context.User.Include(x=>x.StoredReply).FirstOrDefault(x => x.Id == _currentUserService.UserId);
                idsString = request.storedReplyList.Split(",");


                foreach (var idString in idsString)
                {
                    Int32.TryParse(idString, out id);
                    if(id != 0)
                    {
                        sr = user.StoredReply.FirstOrDefault(x => x.Id == id);

                        if (sr == null)
                            throw new NotFoundException(nameof(CleanArchitecture.Domain.Entities.StoredReply), id);

                        user.StoredReply.Remove(sr);
                    }
                    else
                    {
                        throw new NotFoundException(nameof(CleanArchitecture.Domain.Entities.StoredReply), id);
                    }
                    id = 0;
                }

                return await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
