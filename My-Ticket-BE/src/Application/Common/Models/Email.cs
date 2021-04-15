using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Common.Models
{
    public class Email
    {
        public Email()
        {
            attachement = new List<byte[]>();
        }
        public string from { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public ICollection<byte[]> attachement { get; set; }
    }
}
