using CleanArchitecture.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities
{
    public class AppUser : AuditableEntity
    {
        public AppUser()
        {
            StoredReply = new HashSet<StoredReply>();
            UserList = new HashSet<AppUser>();
        }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public int? ZipCode { get; set; }
        public string Locality { get; set; }
        public string Street { get; set; }
        public string ProfilPicture { get; set; }
        public bool IsCompany { get; set; }
        public string? CompanyName { get; set; }
        public bool IsEmployee { get; set; }
        public string Signature { get; set; }
        public string Language { get; set; }
        public virtual Group Group { get; set; }
        public virtual ICollection<StoredReply> StoredReply { get; private set; }
        public virtual ICollection<AppUser> UserList { get; private set; }
    }
}
