using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTicketWearable.WebApi
{
    public class Token
    {
        public string UserToken { get; set; }
        public string Expiration { get; set; }
        //public string UserRoles { get; set; }
        public string PP { get; set; }
        public string Username { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Signature { get; set; }
    }
}
