using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;

namespace CleanArchitecture.SharedKernel.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public virtual AppUser User { get; set; }
    }
}
