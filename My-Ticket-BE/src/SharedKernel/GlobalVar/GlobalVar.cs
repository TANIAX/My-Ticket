using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.SharedKernel.GlobalVar
{
    public static class GlobalVar
    {
        private static string fileBERelativePath;
        public static string PrivateGoogleCaptchaKey = "6Lf9K64UAAAAANQFYEEM09KQQUqz7IxHEEal-ezm";

        #region email parameters
        public static string Email = "TANIAXDEV@gmail.com";
        public static string EmailPassword = "qOZ5Hplwi0ISZeBoLKxR";
        #endregion

        #region SMTP email parameters
        public static string SMTPServerAdress = "smtp.gmail.com";
        public static int SMTPPort = 587;
        #endregion

        #region iMAP email parameters
        public static string ImapServerAdress = "imap.gmail.com";
        public static int ImapPort = 993;
        #endregion

        #region frontend parameters
        public static string AssetsFEAbsolutePath = @"C:\Users\Guillaume\OneDrive\Documents\Programmation\Web\Angulartest\My-Ticket\My-Ticket-FE\src\assets\";
        public static string AssetsFERelativePath = @"..\..\..\assets\";
        public static string FrontendBaseUrl = @"http://localhost:4200/";
        #endregion
        public static string FileBERelativePath
        {
            get { return fileBERelativePath; }
            set 
            { 
                fileBERelativePath = value.Substring(value.IndexOf("WebUI\\wwwroot") + 6); 
            }
        }



        public static string FEUrl = @"http://localhost:4200/";
        public static class Cloudinary
        {
            public static string Cloud = "doifcljfo";
            public static string ApiKey = "569479532991157";
            public static string ApiSecret = "FTjF66c73iJPkOo5IEa7UZwbb-o";
        }
    }
}
