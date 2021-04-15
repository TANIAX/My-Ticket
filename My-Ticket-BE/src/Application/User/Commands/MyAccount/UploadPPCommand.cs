//using CleanArchitecture.Application.Common.Interfaces;
//using CleanArchitecture.Domain.Entities;
//using CleanArchitecture.SharedKernel.GlobalVar;
//using CleanArchitecture.SharedKernel.Helper;
//using MediatR;
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace CleanArchitecture.Application.User.Commands.MyAccount
//{
//    public class UploadPPCommand : IRequest<int>
//    {
//        public string imageData { get; set; }

//        public class UploadPPCommandHandler : IRequestHandler<UploadPPCommand, int>
//        {
//            private readonly IApplicationDbContext _context;
//            private readonly ICloudinary _cloudinary;
//            private readonly ICurrentUserService _currentUserService;
//            public  UploadPPCommandHandler(IApplicationDbContext context, ICloudinary cloudinary, ICurrentUserService currentUserService)
//            {
//                _context = context;
//                _cloudinary = cloudinary;
//                _currentUserService = currentUserService;
//            }
//            public async Task<int> Handle(UploadPPCommand request, CancellationToken cancellationToken)
//            {
//                Image image;
//                AppUser user;
//                string userId;
//                string filePath;

//                userId = _currentUserService.UserId;
//                filePath = GlobalVar.FileBERelativePath + "\\App_Data\\" + userId + ".png";
//                image = Helper.Base64ToImage(request.imageData);
//                user = _context.User.FirstOrDefault(x => x.Id == userId);

//                using(image = new Bitmap(image, 150, 150))
//                {
//                    image.Save(filePath);
//                    image.Dispose();
//                }

//                string result = _cloudinary.UploadToCloudinary(filePath, userId, "Profil Picture");

//                if (result != "Error")
//                {
//                    user.ProfilPicture = result;

//                    Helper.DeleteLocalFile(filePath); 
//                }
//                return await _context.SaveChangesAsync(cancellationToken);
//            }
            
//        }
//    }
//}
