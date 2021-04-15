using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<TodoList> TodoLists { get; set; }

        DbSet<TodoItem> TodoItems { get; set; }

        DbSet<CleanArchitecture.Domain.Entities.AppUser> User { get; set; }
        DbSet<CleanArchitecture.Domain.Entities.Project> Project { get; set; }
        DbSet<TicketHeader> TickerHeader { get; set; }
        DbSet<TicketLine> TicketLine { get; set; }
        DbSet<CleanArchitecture.Domain.Entities.Priority> Priority { get; set; }
        DbSet<CleanArchitecture.Domain.Entities.Status> Status { get; set; }
        DbSet<CleanArchitecture.Domain.Entities.Type> Type { get; set; }
        DbSet<CleanArchitecture.Domain.Entities.Group> Group { get; set; }
        DbSet<CleanArchitecture.Domain.Entities.StoredReply> StoredReplies { get; set; }
        DbSet<CleanArchitecture.Domain.Entities.Satisfaction> Satisfaction { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
