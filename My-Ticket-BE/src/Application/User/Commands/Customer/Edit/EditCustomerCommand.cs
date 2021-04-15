using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.SharedKernel.Helper;
using CleanArchitecture.SharedKernel.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.User.Commands.Customer.Edit
{
    public class EditCustomerCommand : IRequest<int>
    {
        public string Id { get; set; }
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
        public class CreateCustomerCommandHandler : IRequestHandler<EditCustomerCommand, int>
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
            public async Task<int> Handle(EditCustomerCommand request, CancellationToken cancellationToken)
            {
                AppUser customer;

                customer = await _context.User.FirstOrDefaultAsync(x => x.Id == request.Id);

                customer.LastName = request.LastName;
                customer.FirstName = request.FirstName;
                customer.PhoneNumber = request.PhoneNumber;
                customer.CompanyName = request.CompanyName;
                customer.Country = request.Country;
                customer.District = request.District;
                customer.Locality = request.Locality;
                customer.ZipCode = request.ZipCode;
                customer.Street = request.Street;

                return await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
