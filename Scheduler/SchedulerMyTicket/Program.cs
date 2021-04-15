using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SchedulerMyTicket
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        static void Main(string[] args)
        {
            Console.Title = "My ticket Scheduler";
            Properties config = new Properties(AppDomain.CurrentDomain.BaseDirectory + "\\config.prop");
            var handle = GetConsoleWindow();
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(Convert.ToInt32(config.get("Timer")));
            Console.WriteLine("******* CONFIG *******");
            Console.WriteLine("ShowConsole="+config.get("ShowConsole"));
            Console.WriteLine("Timer="+config.get("Timer"));
            Console.WriteLine("ApiEndPointUrl="+config.get("ApiEndPointUrl"));
            Console.WriteLine("**********************");
            if (config.get("ShowConsole") == "True") ShowWindow(handle, SW_SHOW);
            else if (config.get("ShowConsole") == "False") ShowWindow(handle, SW_HIDE);

            var timer = new System.Threading.Timer((e) =>
            {
                CallApiEndPoint(config.get("ApiEndPointUrl"));
            }, null, startTimeSpan, periodTimeSpan);

            while (true)
            {

            }


        }
        public static void CallApiEndPoint(string url)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Request " + DateTime.Now); 
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.Method = "GET";
            webrequest.ContentType = "application/x-www-form-urlencoded";

            try
            {
                HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
                if(webresponse.StatusCode == HttpStatusCode.OK)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("OK");
                    Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
                    StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
                    string result = string.Empty;
                    result = responseStream.ReadToEnd();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error");
                    WriteLog(DateTime.Now+" : "+webresponse.StatusDescription, (@".\log\"));
                }    
                webresponse.Close();
                Console.WriteLine();
            }catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error");
                WriteLog(DateTime.Now + " : " + e.Message, (@".\log\"));
            }

        }
        public static void WriteLog(string strLog, string path)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            path = path + "Log.txt";
            logFileInfo = new FileInfo(path);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(path, FileMode.Append);
            }
            log = new StreamWriter(fileStream);
            log.WriteLine(strLog);
            log.Close();
        }
    }
}
