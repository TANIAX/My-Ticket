using CleanArchitecture.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities
{
    public class Group : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
