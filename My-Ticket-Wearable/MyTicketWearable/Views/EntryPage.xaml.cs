using System;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;

namespace MyTicketWearable.Views
{
    // Define an alias for Tizen PlatformConfiguration.
    using Tizen = Xamarin.Forms.PlatformConfiguration.Tizen;

    /// <summary>
    /// Xamarin.Forms Entry is used for single-line text input. Entry supports various keyboard types.
    /// For more details, see https://docs.microsoft.com/xamarin/xamarin-forms/user-interface/text/entry.
    /// </summary>
    public partial class EntryPage : ContentPage
    {
        public EntryPage()
        {
            InitializeComponent();
        }

        private void OnBottomButtonClicked(object sender, EventArgs e)
        {
            // TODO: Insert code to handle the bottom button click.
            if (Email.Text.Length > 3 && Password.Text.Length > 3 && ApiUrl.Text.Length > 3)
            {
                Xamarin.Forms.Application.Current.Properties["Email"] = Email.Text;
                Xamarin.Forms.Application.Current.Properties["Password"] = Password.Text;
                Xamarin.Forms.Application.Current.Properties["ApiUrl"] = ApiUrl.Text;

                Xamarin.Forms.Application.Current.SavePropertiesAsync();

                Navigation.PushAsync(new GridLayoutPage());
            }
        }
    }
}
