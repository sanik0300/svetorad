using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.StyleSheets;

namespace svetotelegraf
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            showtext.IsChecked = Preferences.Get("showtext", true);
            musicok.IsChecked = MainPage.setting_allows_beep_always = Preferences.Get("musicok", true);
            nochange.IsChecked = MainPage.setting_locks_beeper = Preferences.Get("nochange", false);

            musicok.CheckedChanged += Musicok_CheckedChanged;
            nochange.CheckedChanged += Nochange_CheckedChanged;
        }

        private void setting_check(object sender, EventArgs e)
        {
            Preferences.Set((sender as View).StyleId, (e as CheckedChangedEventArgs).Value);
        }
        private void Musicok_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            MainPage.setting_allows_beep_always = e.Value;
        }

        private void Nochange_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            MainPage.setting_locks_beeper = e.Value;
        }

    }
}