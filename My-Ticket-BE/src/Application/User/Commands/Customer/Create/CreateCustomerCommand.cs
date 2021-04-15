using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.SharedKernel.Helper;
using CleanArchitecture.SharedKernel.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.User.Commands.Customer.Create
{
    public class CreateCustomerCommand : IRequest<object>
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string Locality { get; set; }
        public int ZipCode { get; set; }
        public string Street { get; set; }
        public bool Exist { get; set; }
        public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, object>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            private readonly IIdentityService _userManager;
            private readonly ICloudinary _cloudinaryService;
            private readonly IEmail _email;

            public CreateCustomerCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService,
                IIdentityService userManager, ICloudinary cloudinaryService, IEmail email)
            {
                _context = context;
                _currentUserService = currentUserService;
                _userManager = userManager;
                _cloudinaryService = cloudinaryService;
                _email = email;
            }
            public async Task<object> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
            {
                IdentityResult result;
                ApplicationUser customer;
                AppUser customerUser;
                string password, defaultPP, PPLink;

                customer = new ApplicationUser()
                {
                    UserName = request.Email,
                    EmailConfirmed = true,
                    NormalizedEmail = request.Email.ToUpper(),
                    Email = request.Email,
                };
                password = Helper.GeneratePassword(8);
                try
                {
                    result = await _userManager.CreateAsync(customer, password);
                    if (!result.Succeeded)
                    {
                        throw new ApplicationException(_userManager.IdentityResultError(result));
                    }
                    else
                    {
                        customerUser = new AppUser
                        {
                            Id = customer.Id,
                            UserName = request.Email,
                            LastName = request.LastName,
                            FirstName = request.FirstName,
                            PhoneNumber = request.PhoneNumber,
                            Email = request.Email,
                            IsCompany = true,
                            CompanyName = request.CompanyName,
                            Country = request.Country,
                            District = request.District,
                            Locality = request.Locality,
                            ZipCode = request.ZipCode,
                            Street = request.Street
                        };
                        customer.User = customerUser;

                        result = await _userManager.AddToRoleAsync(customer, "Customer");
                        if (!result.Succeeded)
                        {
                            await _userManager.DeleteAsync(customer);
                            throw new ApplicationException(_userManager.IdentityResultError(result));
                        }

                        defaultPP = Helper.CreateDefaultPP(customerUser.Id, customerUser.FirstName, customerUser.LastName);
                        PPLink = "";
                        if (defaultPP != "Error")
                        {
                            PPLink = _cloudinaryService.UploadToCloudinary(defaultPP, customerUser.Id, "Profil Picture");
                            if (PPLink != "" || PPLink != "Error")
                            {
                                customerUser.ProfilPicture = PPLink;
                            }
                            Helper.DeleteLocalFile(defaultPP);
                        }
                        else
                        {
                            customerUser.ProfilPicture = @"https://res.cloudinary.com/doifcljfo/image/upload/v1581022559/NoIdent_afyvgb.png";
                        }

                        if(await _context.SaveChangesAsync(cancellationToken) > 0 )
                            _email.SendEmail(request.Email,"Account created","Hello "+request.FirstName+"" +
                                                                             ".<br> You now have an account on My ticket. here's your connexion data : " +
                                                                             "<br> Login : "+ request.Email + "<br> Password :" +password + "<br>" +
                                                                             "Kind regards. <br>My ticket team.");
                        return await Task.FromResult(new { Id = customerUser.Id });
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
