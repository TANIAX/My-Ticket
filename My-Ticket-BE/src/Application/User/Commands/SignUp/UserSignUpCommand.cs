using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.SharedKernel.Helper;
using CleanArchitecture.SharedKernel.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.User.Commands.SignUp
{
    public partial class UserSignUpCommand : IRequest<bool>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public class UserSignUpCommandHandler : IRequestHandler<UserSignUpCommand, bool>
        {
            private readonly IApplicationDbContext _context;
            private readonly IEmail _email;
            private readonly IIdentityService _userManager;
            private readonly ICloudinary _cloudinaryService;

            public UserSignUpCommandHandler(IApplicationDbContext context,
                IEmail email,
                IIdentityService userManager,
                ICloudinary cloudinaryService)
            {
                _context = context;
                _userManager = userManager;
                _email = email;
                _cloudinaryService = cloudinaryService;
            }

            public async Task<bool> Handle(UserSignUpCommand request, CancellationToken cancellationToken)
            {
                IdentityResult result;
                AppUser appUser;
                ApplicationUser user;
                string defaultPP;
                string PPLink;
                List<TicketHeader> tl;

                user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                };

                try
                {
                    result = await _userManager.CreateAsync(user, request.Password);
                    if (!result.Succeeded)
                    {
                        throw new ApplicationException(_userManager.IdentityResultError(result));
                    }
                    else
                    {
                        appUser = new AppUser
                        {
                            Id = user.Id,
                            UserName = request.Email,
                            LastName = request.LastName,
                            FirstName = request.FirstName,
                            Email = request.Email,
                        };

                        _context.User.Add(appUser);
                        await _context.SaveChangesAsync(cancellationToken);

                        user.User = appUser;

                        result = await _userManager.AddToRoleAsync(user, "Customer");
                        if (!result.Succeeded)
                        {
                            await _userManager.DeleteAsync(user);
                            throw new ApplicationException(_userManager.IdentityResultError(result));
                        }
                    }


                    defaultPP = Helper.CreateDefaultPP(appUser.Id, appUser.FirstName, appUser.LastName);
                    PPLink = "";
                    if (defaultPP != "Error")
                    {
                        PPLink = _cloudinaryService.UploadToCloudinary(defaultPP, appUser.Id, "Profil Picture");
                        if (PPLink != "" || PPLink != "Error")
                        {
                            appUser.ProfilPicture = PPLink;
                        }
                        Helper.DeleteLocalFile(defaultPP);
                    }
                    else
                    {
                        appUser.ProfilPicture = @"https://res.cloudinary.com/doifcljfo/image/upload/v1581022559/NoIdent_afyvgb.png";
                    }


                    //Associate ticket is previous ticket
                    tl = _context.TickerHeader.Where(x=> x.Email == appUser.Email).ToList();

                    foreach(var t in tl)
                    {
                        t.Requester = appUser;
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    throw e;
                }

                return true;
            }

        }
    }
}
