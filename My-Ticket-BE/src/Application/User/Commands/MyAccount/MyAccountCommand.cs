using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Drawing;
using CleanArchitecture.SharedKernel.Helper;
using CleanArchitecture.SharedKernel.GlobalVar;

namespace CleanArchitecture.Application.User.Commands.MyAccount
{
    public class MyAccountCommand : IRequest<int>
    {
        public string Signature { get; set; }
        public string Language { get; set; }
        public string imageData { get; set; }

        public class MyAccountCommandHandler : IRequestHandler<MyAccountCommand,int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICloudinary _cloudinary;
            private readonly ICurrentUserService _currentUserService;

            public MyAccountCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, ICloudinary cloudinary)
            {
                _context = context;
                _cloudinary = cloudinary;
                _currentUserService = currentUserService;
            }
            public async Task<int> Handle(MyAccountCommand request, CancellationToken cancellationToken)
            {
                Image image;
                string userId;
                string filePath;
                AppUser appUser;

                userId = _currentUserService.UserId;
                appUser = _context.User.FirstOrDefault(x => x.Id == userId);
                if (request.imageData.Length > 0)
                {
                    filePath = GlobalVar.FileBERelativePath + "\\App_Data\\" + userId + ".png";
                    image = Helper.Base64ToImage(request.imageData);

                    using (image = new Bitmap(image, 150, 150))
                    {
                        image.Save(filePath);
                        image.Dispose();
                    }

                    string result = _cloudinary.UploadToCloudinary(filePath, userId, "Profil Picture");

                    if (result != "Error")
                    {
                        appUser.ProfilPicture = result;

                        Helper.DeleteLocalFile(filePath);
                    }
                }
 
                if (request.Language.Length > 0)
                    appUser.Language = request.Language;

                appUser.Signature = request.Signature;

                return await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
