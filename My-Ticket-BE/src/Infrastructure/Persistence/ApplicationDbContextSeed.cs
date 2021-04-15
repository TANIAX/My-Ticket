using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.SharedKernel.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            ApplicationDbContext _context = context;

            var defaultUser = new ApplicationUser
            {
                UserName = "Guillaume.taniax@gmail.com",
                Email = "Guillaume.taniax@gmail.com",
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                await userManager.CreateAsync(defaultUser, "CleanArchitecture!");
            }

            IdentityRole customer = new IdentityRole
            {
                Name = "Customer"
            };

            IdentityRole employee = new IdentityRole
            {
                Name = "Member"
            };

            IdentityRole admin = new IdentityRole
            {
                Name = "Admin"
            };

            await roleManager.CreateAsync(customer);
            await roleManager.CreateAsync(employee);
            await roleManager.CreateAsync(admin);

            await userManager.AddToRoleAsync(defaultUser, "Admin");
            await userManager.AddToRoleAsync(defaultUser, "Member");
            await userManager.AddToRoleAsync(defaultUser, "Customer");

            //var open = new Status
            //{
            //    Name = "Open",
            //};
            //_context.Status.Add(open);
            //var pending = new Status
            //{
            //    Name = "Pending",
            //};
            //_context.Status.Add(pending);
            //var closed = new Status
            //{
            //    Name = "Closed",
            //};
            //_context.Status.Add(closed);
            //var waitingOnCustomer = new Status
            //{
            //    Name = "Waiting on customer",
            //};
            //_context.Status.Add(waitingOnCustomer);
            //var waitingOnThirdParty = new Status
            //{
            //    Name = "Waiting on third party",
            //};
            //_context.Status.Add(waitingOnThirdParty);

            //_context.SaveChanges();
        }
    }
}
