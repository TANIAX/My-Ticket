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

namespace CleanArchitecture.Application.StoredReply.Commands.Create
{
    public class CreateStoredReplyCommand : IRequest<int>
    {
        public string Title { get; set; }
        public string Reply { get; set; }
        public class CreateStoredReplyCommandHandler : IRequestHandler<CreateStoredReplyCommand, int>
        {
            IApplicationDbContext _context;
            ICurrentUserService _currentUserService;
            public CreateStoredReplyCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(CreateStoredReplyCommand request, CancellationToken cancellationToken)
            {
                CleanArchitecture.Domain.Entities.StoredReply sr;
                AppUser user;

                sr = new CleanArchitecture.Domain.Entities.StoredReply
                {
                    Title = request.Title,
                    Reply = request.Reply
                };

                user = _context.User.Include(x=>x.StoredReply).FirstOrDefault(x => x.Id == _currentUserService.UserId);
                user.StoredReply.Add(sr);

                await _context.SaveChangesAsync(cancellationToken);

                return sr.Id;
            }
        }
    }
}
