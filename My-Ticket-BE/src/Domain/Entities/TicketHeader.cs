using CleanArchitecture.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities
{
    public class TicketHeader : AuditableEntity 
    {
        public TicketHeader()
        {
            this.TicketLine = new LinkedList<TicketLine>();
        }
        public const int ReadedByMemberOrAdmin = 1;
        public const int ReadedByCustomer = 2;
        public int Id { get; set; }
        public string Title { get; set; }
        public string InternTitle { get; set; }
        public string Description { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string Email { get; set; }
        public int Readed { get; set; }
        public virtual AppUser AssignTO { get; set; }
        public virtual AppUser Requester { get; set; }
        public virtual Group Group { get; set; }
        public virtual Status Status { get; set; }
        public virtual Priority Priority { get; set; }   
        public virtual Project Project { get; set; }
        public virtual Type Type { get; set; }
        public virtual Satisfaction Satisfaction { get; set; }
        public virtual ICollection<TicketLine> TicketLine { get; private set; }
        
        
    }
}
