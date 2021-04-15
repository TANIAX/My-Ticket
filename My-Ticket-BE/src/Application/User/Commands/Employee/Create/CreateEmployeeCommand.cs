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

namespace CleanArchitecture.Application.User.Commands.Employee.Create
{
    public class CreateEmployeeCommand : IRequest<object>
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, object>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            private readonly IIdentityService _userManager;
            private readonly ICloudinary _cloudinaryService;
            private readonly IEmail _email;

            public CreateEmployeeCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService,
                IIdentityService userManager, ICloudinary cloudinaryService, IEmail email)
            {
                _context = context;
                _currentUserService = currentUserService;
                _userManager = userManager;
                _cloudinaryService = cloudinaryService;
                _email = email;
            }
            public async Task<object> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
            {
                IdentityResult result;
                ApplicationUser employee;
                AppUser appEmployeeUser, currentUser;
                string password, defaultPP, PPLink;

                currentUser = _context.User.FirstOrDefault(x => x.Id == _currentUserService.UserId);

                employee = new ApplicationUser()
                {
                    UserName = request.Email,
                    EmailConfirmed = true,
                    NormalizedEmail = request.Email.ToUpper(),
                    Email = request.Email,
                };
                password = Helper.GeneratePassword(8);
                try
                {
                    result = await _userManager.CreateAsync(employee, password);
                    if (!result.Succeeded)
                    {
                        throw new ApplicationException(_userManager.IdentityResultError(result));
                    }
                    else
                    {
                        appEmployeeUser = new AppUser
                        {
                            Id = employee.Id,
                            UserName = request.Email,
                            LastName = request.LastName,
                            FirstName = request.FirstName,
                            PhoneNumber = request.PhoneNumber,
                            Email = request.Email,
                            IsEmployee = true,
                            CompanyName = currentUser.CompanyName
                        };
                        employee.User = appEmployeeUser;

                        result = await _userManager.AddToRoleAsync(employee, "Customer");
                        if (!result.Succeeded)
                        {
                            await _userManager.DeleteAsync(employee);
                            throw new ApplicationException(_userManager.IdentityResultError(result));
                        }

                        defaultPP = Helper.CreateDefaultPP(appEmployeeUser.Id, appEmployeeUser.FirstName, appEmployeeUser.LastName);
                        PPLink = "";
                        if (defaultPP != "Error")
                        {
                            PPLink = _cloudinaryService.UploadToCloudinary(defaultPP, appEmployeeUser.Id, "Profil Picture");
                            if (PPLink != "" || PPLink != "Error")
                            {
                                appEmployeeUser.ProfilPicture = PPLink;
                            }
                            Helper.DeleteLocalFile(defaultPP);
                        }
                        else
                        {
                            appEmployeeUser.ProfilPicture = @"https://res.cloudinary.com/doifcljfo/image/upload/v1581022559/NoIdent_afyvgb.png";
                        }
                        currentUser.UserList.Add(appEmployeeUser);

                        if(await _context.SaveChangesAsync(cancellationToken) > 0 )
                            _email.SendEmail(request.Email,"Account created","Hello "+request.FirstName+"" +
                                                                             ".<br> You now have an account on My ticket. here's your connexion data : " +
                                                                             "<br> Login : "+ request.Email + "<br> Password :" +password + "<br>" +
                                                                             "Kind regards. <br>My ticket team.");
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

                return await Task.FromResult(new { Id = appEmployeeUser.Id });

            }
        }
    }
}
