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

namespace CleanArchitecture.Application.StoredReply.Commands.Update
{
    public class UpdateStoredReplyCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Reply { get; set; }
        public class UpdateStoredReplyCommandHandler : IRequestHandler<UpdateStoredReplyCommand, int>
        {
            IApplicationDbContext _context;
            ICurrentUserService _currentUserService;
            public UpdateStoredReplyCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(UpdateStoredReplyCommand request, CancellationToken cancellationToken)
            {
                CleanArchitecture.Domain.Entities.StoredReply sr;
                AppUser user;

                user = _context.User.Include(x => x.StoredReply).FirstOrDefault(x => x.Id == _currentUserService.UserId);
                sr = user.StoredReply.FirstOrDefault(x => x.Id == request.Id);

                if (sr == null)
                    throw new NotFoundException(nameof(CleanArchitecture.Domain.Entities.StoredReply), request.Id);

                sr.Title = request.Title;
                sr.Reply = request.Reply;

                return await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
