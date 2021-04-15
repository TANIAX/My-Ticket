using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Ticket.Queries.Wearable.GetUnreadAndOpenNumber
{
    public class UnreadAndOpenDTO
    {
        public int Unread { get; set; }
        public int Open { get; set; }
        public int PriorityHightOrUrgent { get; set; }
    }
}
