using CleanArchitecture.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities
{
    public class StoredReply : AuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Reply { get; set; }

        public override bool Equals(object obj)
        {
            StoredReply sr = obj as StoredReply;

            if (sr == null)
            {
                return false;
            }

            return sr.Title == this.Title && sr.Reply == sr.Reply;
        }
        public override int GetHashCode()
        {
            return string.Format("{0}_{1}", Title, Reply).GetHashCode();
        }
    }
}
