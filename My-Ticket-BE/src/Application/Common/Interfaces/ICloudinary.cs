using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Common.Interfaces
{
    public interface ICloudinary
    {
        public string UploadToCloudinary(string filePath, string fileName, string fileDescription);
        public List<string> UploadToCloudinary(List<byte[]> imageBase64);
    }
}
