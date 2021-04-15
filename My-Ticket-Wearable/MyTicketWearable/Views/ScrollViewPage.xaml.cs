using System;

using Xamarin.Forms;

namespace MyTicketWearable.Views
{
    public partial class ScrollViewPage : ContentPage
    {
        public ScrollViewPage(string title, string description)
        {
            InitializeComponent();
            Title.Text = title;
            Description.Text = description;
        }
    }
}
