using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.SharedKernel.GlobalVar;
using CleanArchitecture.SharedKernel.Helper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Upload
{
    public class UploadCommand : IRequest<object>
    {
        public string FileName { get; set; }
        public Stream Stream { get; set; }

        public UploadCommand(string fileName, Stream stream)
        {
            this.FileName = fileName;
            this.Stream = stream;
        }

        public class UploadCommandHandler : IRequestHandler<UploadCommand, object>
        {
            ICloudinary _cloudinary;
            
            public UploadCommandHandler(ICloudinary cloudinary)
            {
                _cloudinary = cloudinary;
            }

            public async Task<object> Handle(UploadCommand request, CancellationToken cancellationToken)
            {
                string filePath = "", guid = "";
                Image image = null;

                if (!Helper.FileIsImage(request.FileName))
                    throw new UnsupportedException("File",request.FileName);
                guid = Helper.GenerateGUID();
                filePath = GlobalVar.FileBERelativePath + "\\App_Data\\" + guid + Path.GetExtension(request.FileName);

                using (var ms  = request.Stream)
                {
                    image = Image.FromStream(ms);
                    image.Save(filePath);
                    image.Dispose();
                }

                string result = _cloudinary.UploadToCloudinary(filePath, guid, "TicketContent");

                if (result != "Error")
                {
                    Helper.DeleteLocalFile(filePath);
                    return new { url = result };
                }

                return new { }; 
            }
        }
    }
}
