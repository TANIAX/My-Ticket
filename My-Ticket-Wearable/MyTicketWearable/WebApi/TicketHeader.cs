using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTicketWearable.WebApi
{
    public class TicketHeader
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
    
}
