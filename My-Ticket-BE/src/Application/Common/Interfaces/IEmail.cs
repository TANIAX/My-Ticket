using CleanArchitecture.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common.Interfaces
{
    public interface IEmail
    {

        public bool MailIsValid(string email);
        public void SendEmail(string userEmail, string subject, string body);
        public IList<Email> ReadInbox();
        //public bool AttachementIsImage(byte[] attachement);
    }
}
