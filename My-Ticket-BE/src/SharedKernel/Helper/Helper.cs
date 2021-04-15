using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using CleanArchitecture.SharedKernel.GlobalVar;
using MimeKit;

namespace CleanArchitecture.SharedKernel.Helper
{
    public static class Helper
    {
        public static bool DeleteLocalFile(string filePath)
        {
            try
            {
                // Check if file exists with its full path    
                if (File.Exists(filePath))
                {
                    // If file found, delete it    
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public static string GenerateGUID()
        {
            return Guid.NewGuid().ToString("N");
        }
        public static bool FileIsImage(string file)
        {
            string[] needles = new string[5] { ".jpeg", ".jpg", ".bmp", ".png", ".tiff" };
            foreach (string needle in needles)
            {
                if (file.ToLower().Contains(needle.ToLower()))
                    return true;
            }

            return false;
        }
        public static string CreateDefaultPP(string userId, string firstname, string lastname)
        {
            try
            {
                string name = firstname[0].ToString().ToUpper() + lastname[0].ToString().ToUpper();
                Font font = new Font(FontFamily.GenericSerif, 60, FontStyle.Bold);
                Color fontcolor = ColorTranslator.FromHtml("#FFF");
                Color bgcolor = ColorTranslator.FromHtml(GetRandomColor());

                //first, create a dummy bitmap just to get a graphics object  
                Image img = new Bitmap(1, 1);
                Graphics drawing = Graphics.FromImage(img);

                //measure the string to see how big the image needs to be  
                SizeF textSize = drawing.MeasureString(name, font);

                //free up the dummy image and old graphics object  
                img.Dispose();
                drawing.Dispose();

                //create a new image of the right size  
                img = new Bitmap(150, 150);

                drawing = Graphics.FromImage(img);

                //paint the background  
                drawing.Clear(bgcolor);

                //create a brush for the text  
                Brush textBrush = new SolidBrush(fontcolor);

                //drawing.DrawString(text, font, textBrush, 0, 0);  
                drawing.DrawString(name, font, textBrush, new Rectangle(5, 30, 200, 110));

                drawing.Save();

                textBrush.Dispose();
                drawing.Dispose();
                var path = GlobalVar.GlobalVar.FileBERelativePath + "\\App_Data\\" + userId + ".png";
                img.Save(path);
                return path;
            }
            catch
            {
                return "Error";
            }
        }

        public static string GeneratePassword(int passwordLength)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < passwordLength--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public static string GetRandomColor()
        {
            Random random = new Random();

            List<string> list = new List<string>();
            list.Add("#EEAD0E");
            list.Add("#8bbf61");

            list.Add("#DC143C");
            list.Add("#CD6889");
            list.Add("#8B8386");
            list.Add("#800080");
            list.Add("#9932CC");
            list.Add("#009ACD");
            list.Add("#00CED1");
            list.Add("#03A89E");

            list.Add("#00C78C");
            list.Add("#00CD66");
            list.Add("#66CD00");
            list.Add("#EEB422");
            list.Add("#FF8C00");
            list.Add("#EE4000");

            list.Add("#388E8E");
            list.Add("#8E8E38");
            list.Add("#7171C6");

            int index = random.Next(list.Count);

            return list[index];
        }
        public static bool isMajor(DateTime birthdate)
        {

            DateTime today = DateTime.Today;
            int age = today.Year - birthdate.Year;
            if (birthdate > today.AddYears(-age))
                return true;
            else
                return false;
        }
        public static Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                ms.Write(imageBytes, 0, imageBytes.Length);
                return Image.FromStream(ms, true);
            } 
        }
    }
}
