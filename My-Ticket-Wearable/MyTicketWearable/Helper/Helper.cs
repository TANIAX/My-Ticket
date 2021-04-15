using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Essentials;

namespace MyTicketWearable.Helper
{
    public static class Helper
    {

        public static bool InternetActive()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
                return true;
            else
                return false;


        }
        public static bool isWeekend()
        {
            DateTime date = DateTime.Now;
            DayOfWeek day = DateTime.Now.DayOfWeek;

            if ((day == DayOfWeek.Saturday) || (day == DayOfWeek.Sunday))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty).Replace("&nbsp;", " ");
        }
    }
}
    
        
