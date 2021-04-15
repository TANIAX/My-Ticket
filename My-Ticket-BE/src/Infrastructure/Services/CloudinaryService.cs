
using CleanArchitecture.Application.Common.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;
using CleanArchitecture.SharedKernel.Helper;
using CleanArchitecture.SharedKernel.GlobalVar;

namespace CleanArchitecture.Infrastructure.Services
{
    public static class CloudinaryAccess
    {
        public static string cloud = "doifcljfo";
        public static string apiKey = "569479532991157";
        public static string apiSecret = "FTjF66c73iJPkOo5IEa7UZwbb-o";
    }
    public class CloudinaryService : ICloudinary
    {
        Cloudinary cloudinary = new Cloudinary(new Account(CloudinaryAccess.cloud, CloudinaryAccess.apiKey, CloudinaryAccess.apiSecret));
        
        public string UploadToCloudinary(string filePath, string fileName, string fileDescription)
        {
             var result = cloudinary.Upload(new ImageUploadParams
            {
                File = new FileDescription(fileDescription, filePath),
                PublicId = fileName
            });
            if (result.Uri != null)
            {
                return result.Uri.ToString();
            }
            else
            {
                return "Error";
            }
        }
        public List<string> UploadToCloudinary(List<byte[]> imageBase64)
        {
            List<string> list = new List<string>();
            string name;
            string path;
            Image image;

            foreach (var imageBytes in imageBase64)
            {
                // Convert byte[] to Image
                using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    image = Image.FromStream(ms, true);
                    name = Helper.GenerateGUID();
                    path = GlobalVar.FileBERelativePath + "\\" + name + ".jpeg";
                    image.Save(path);
                    image.Dispose();

                    var result = cloudinary.Upload(new ImageUploadParams
                    {
                        File = new FileDescription("TicketFile", path),
                        PublicId = name
                    });

                    if (result.Uri != null)
                    {
                        list.Add("<img src='" + result.Uri.ToString() +"'/>");
                        Helper.DeleteLocalFile(path);
                    }

                }

            }
            return list;
        }
       
    }
}
