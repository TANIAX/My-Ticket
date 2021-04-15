using MyTicketWearable.WebApi;
using Nancy.Json;
using System;
using System.IO;
using System.Net;
using Tizen.Applications.Notifications;
using Xamarin.Forms;
using static MyTicketWearable.Helper.Helper;

namespace MyTicketWearable.Views
{
    public partial class GridLayoutPage : ContentPage
    {
        MyTicketApi api;
        public GridLayoutPage()
        {
            InitializeComponent();
            //Refresh();
        }

        private void onTappedUnread(object sender, EventArgs e)
        {
            if (api != null)
                Navigation.PushAsync(new TextListPage(api,1));
            else
                Refresh();
        }

        private void onTappedOpen(object sender, EventArgs e)
        {
            if (api != null)
                Navigation.PushAsync(new TextListPage(api,2));
            else
                Refresh();

        }
        private void Refresh()
        {
            if (InternetActive())
            {
                //Update values
                api = new MyTicketApi();
                var unreadAndOpen = api.GetUnreadAndOpen();

                UnreadNumber.Text = unreadAndOpen.Unread.ToString();
                OpenNumber.Text = unreadAndOpen.Open.ToString();
            }
            else
            {
                //Display notification internet error
                Notification notification = new Notification
                {
                    Title = "No internet",
                    Content = "Enable your internet connexion before using this app.",
                    Tag = "internet is offline"
                };

                NotificationManager.Post(notification);

                NotificationManager.DeleteAll();
            }

        }

        private void OnButtonRefreshClicked(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
