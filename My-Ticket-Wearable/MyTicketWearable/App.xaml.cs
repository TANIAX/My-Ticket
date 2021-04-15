using MyTicketWearable.Views;
using System;
using static MyTicketWearable.Helper.Helper;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace MyTicketWearable
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //if (!Application.Current.Properties.ContainsKey("Email") || 
            //    !Application.Current.Properties.ContainsKey("Password") || 
            //    !Application.Current.Properties.ContainsKey("ApiUrl"))
            //{
            //    MainPage = new NavigationPage(new EntryPage());
            //}
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            while (true)
            {
                if (InternetActive())
                {
                    if ((DateTime.Now.Hour > 9 && DateTime.Now.Hour < 18) && !isWeekend())
                    {

                    }
                }
            }
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
