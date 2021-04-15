using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.SharedKernel.Identity;
using MediatR;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.User.Commands.EditMyProfile
{
    public class EditMyProfileCommand : IRequest<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Password { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string Locality { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public bool IsCompany { get; set; }
        public string CompanyName { get; set; }
        public class EditMyProfileCommandHandler : IRequestHandler<EditMyProfileCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            private readonly IIdentityService _userManager;
            public EditMyProfileCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IIdentityService userManager)
            {
                _context = context;
                _currentUserService = currentUserService;
                _userManager = userManager;
            }

            public async Task<int> Handle(EditMyProfileCommand request, CancellationToken cancellationToken)
            {
                AppUser user = null;
                int zipCode = 0;

                if (request.Password != "")
                    await _userManager.SetPassword(_currentUserService.UserId, request.Password);

                user = _context.User.FirstOrDefault(x => x.Id == _currentUserService.UserId);
                Int32.TryParse(request.ZipCode, out zipCode);

                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.PhoneNumber = request.PhoneNumber;
                user.BirthDate = request.BirthDate;
                user.Country = request.Country;
                user.District = request.District;
                user.Locality = request.Locality;
                user.ZipCode = zipCode;
                user.Street = request.Street;
                user.IsCompany = request.IsCompany;
                user.CompanyName = request.CompanyName;
                

                return await _context.SaveChangesAsync(cancellationToken);
                
            }
        }
    }
}
