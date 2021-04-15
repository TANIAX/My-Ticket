using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.SharedKernel.GlobalVar;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using CleanArchitecture.Application.Common.Models;
using MailKit.Net.Imap;
using MailKit;
using System.Linq;
using MailKit.Search;
using MimeKit;
using System.IO;

namespace CleanArchitecture.Infrastructure.Services
{
    public class EmailService : IEmail
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }

        public string PopServer { get; set; }
        public int PopPort { get; set; }
        public string PopUsername { get; set; }
        public string PopPassword { get; set; }

        public bool MailIsValid(string email)
        {
            if (!email.IsNullOrEmpty())
            {
                MailAddress m;

                try
                {
                    m = new MailAddress(email);
                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }

            return false;
        }
        public void SendEmail(string userEmail, string subject, string body)
        {
            try
            {
                SmtpClient client;
                MailAddress from, to;
                MailMessage message;

                client = new SmtpClient(GlobalVar.SMTPServerAdress, GlobalVar.SMTPPort);
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(GlobalVar.Email, GlobalVar.EmailPassword);
                from = new MailAddress("Guillaume.cornez@gmail.com", String.Empty, System.Text.Encoding.UTF8);

                to = new MailAddress(userEmail);
                message = new MailMessage(from, to);
                message.IsBodyHtml = true;
                message.Body = body;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Subject = subject;
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                client.Timeout = 30000;
                client.Send(message);
            }
            catch
            {
            }
        }
        public IList<Email> ReadInbox()
        {
            var emailList = new List<Email>();
            Email email;
            string fileName;

            using (var client = new ImapClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(GlobalVar.ImapServerAdress, GlobalVar.ImapPort, true);

                client.Authenticate(GlobalVar.Email, GlobalVar.EmailPassword);

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadWrite);
                
                foreach (var uid in inbox.Search(SearchQuery.NotSeen)) {
                    var message = inbox.GetMessage(uid);
                    inbox.AddFlags(uid, MessageFlags.Seen, true);
                    email = new Email();
                    email.from = message.From.Mailboxes.FirstOrDefault().Address;
                    email.Subject = message.Subject;
                    email.Body = message.TextBody.Replace(System.Environment.NewLine, "<br>");
                    foreach (MimeEntity attachment in message.Attachments)
                    {
                        fileName = attachment.ContentDisposition?.FileName ?? attachment.ContentType.Name;
                        if (FileIsImage(fileName))
                        {
                            using (var memory = new MemoryStream())
                            {
                                if (attachment is MimePart)
                                    ((MimePart)attachment).Content.DecodeTo(memory);
                                else
                                    ((MessagePart)attachment).Message.WriteTo(memory);

                                var bytes = memory.ToArray();
                                email.attachement.Add(bytes);
                            }
                        }
                            
                    }
                    emailList.Add(email);
                    email = null;
                }
                client.Disconnect(true);

                return emailList;
            }
        }
        public bool FileIsImage(string fileName)
        {
            string[] needles = new string[5] { "jpeg", "jpg", "bmp", "png", "tiff" };
            foreach (string needle in needles)
            {
                if (fileName.Contains(needle.ToLower()))
                    return true;
            }

            return false;
        }
    }
}
