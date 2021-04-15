using System;

using MyTicketWearable.Services;
using MyTicketWearable.WebApi;
using Xamarin.Forms;
using static MyTicketWearable.Helper.Helper;

namespace MyTicketWearable.Views
{
    public partial class TextListPage : ContentPage
    {
        MyTicketApi api;
        public TextListPage(MyTicketApi api,int type = 0)
        {
            InitializeComponent();

            // Initialize sample data and set ItemsSource in ListView.
            // TODO: Change ItemsSource with your own data.
            if(api != null)
            {
                this.api = api;
                var lt = api.GetTicketList(type);

                if(lt.Count > 0)
                {
                    listView.ItemsSource = lt;
                    listView.IsVisible = true;
                    noTicketLabel.IsVisible = false;
                    stackLayout.BackgroundColor = Color.Black;
                }
                else
                {
                    listView.IsVisible = false;
                    noTicketLabel.IsVisible = true;
                    stackLayout.BackgroundColor = Color.FromHex("#21D192");
                }
                
                
            }
        }

        // Called once when an item is selected.
        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //  TODO: Insert code to handle a list item selected event.
            // Logger.Info($"Selected Color : {e.SelectedItem}");
        }

        // Called every time an item is tapped.
        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            // TODO: Insert code to handle a list item tapped event.
            Logger.Info($"Tapped Color : {e.Item}");
            if(e.Item != null)
            {
                var ticket = new TicketHeader();
                ticket = (TicketHeader)e.Item;
                

                Navigation.PushAsync(new ScrollViewPage(ticket.Title, Helper.Helper.StripHTML(ticket.Description)));
            }
        }
    }
}
