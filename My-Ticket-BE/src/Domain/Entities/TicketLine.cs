using CleanArchitecture.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities
{
    public class TicketLine : AuditableEntity
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool AskForClose { get; set; }
        public string Email { get; set; }
        public virtual AppUser ResponseBy { get; set; }
    }
}
