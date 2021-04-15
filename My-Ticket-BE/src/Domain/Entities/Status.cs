using CleanArchitecture.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities
{
    public class Status : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Editable { get; set; }
    }
}
