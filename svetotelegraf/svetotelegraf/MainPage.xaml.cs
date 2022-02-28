using Android.OS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace svetotelegraf
{
    public partial class MainPage : ContentPage
    {
        private double width = 0;
        private double height = 0;
        bool horizontal;
        LetterSettings ls = null;
        ISignal signaller;
        public MainPage()
        {
            InitializeComponent();
            width = this.Width;
            height = this.Height;
            signaller = DependencyService.Get<ISignal>();
        }

        private void InstantiatePause()
        {
            lil_buttons.Children.Clear();
            lil_buttons.Children.Add(pause, 0, 0);
            lil_buttons.Children.Add(stop, horizontal ? 0 : 1, horizontal ? 1 : 0);          
        }
        private void InstantiatePlay() {
            lil_buttons.Children.Clear();
            lil_buttons.Children.Add(play);
            if (horizontal)
            {
                Grid.SetRowSpan(play, 2);
                play.HeightRequest = play.Width;
            }
            else
            {
                Grid.SetColumnSpan(play, 2);
                play.WidthRequest = play.Height;
            }
                
            play.HorizontalOptions = horizontal ? LayoutOptions.FillAndExpand : LayoutOptions.Center;
            play.VerticalOptions = horizontal ? LayoutOptions.Center : LayoutOptions.FillAndExpand;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (this.width != width || this.height != height)
            {
                this.width = width;
                this.height = height;
                allingrid.RowDefinitions.Clear();
                allingrid.ColumnDefinitions.Clear();
                allingrid.Children.Remove(ui_stuff);
                lil_buttons.Children.Clear();
                lil_buttons.RowDefinitions.Clear();
                lil_buttons.ColumnDefinitions.Clear();
               
                horizontal = width > height;
                if (horizontal)
                {
                    allingrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(4, GridUnitType.Star) });
                    allingrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                    allingrid.Children.Add(ui_stuff, 1, 0);
                    for (int i = 0; i < 2; i++)
                        lil_buttons.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                }
                else
                {
                    allingrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(3, GridUnitType.Star) });
                    allingrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                    allingrid.Children.Add(ui_stuff, 0, 1);
                    for (int i = 0; i < 2; i++)
                        lil_buttons.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });                                      
                }
                if (ls != null && ls.OnPlay)
                    InstantiatePause();
                else
                    InstantiatePlay();                
            }
        }
        private void chooseVel_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            chooseVel.Value = Math.Round(chooseVel.Value);
            tellVel.Text = $"скорость - {chooseVel.Value} знаков/мин";
        }

        private async void play_Clicked(object sender, EventArgs e)
        {
            if (messagetxt.Text == null)
                return;
            messagetxt.Text = messagetxt.Text.ToLower();

            InstantiatePause();
            chooseVel.IsEnabled = false;
            char[] txts = messagetxt.Text.ToLower().ToCharArray();
            StringBuilder past = new StringBuilder(messagetxt.Text.Length), nexts;
            if (ls == null) { 
                ls = new LetterSettings(messagetxt.Text, (int)chooseVel.Value, false);
                nexts = new StringBuilder(messagetxt.Text, messagetxt.Text.Length);
            }
            else
            {
                ls.position++;
                for(int i =0; i<ls.position; i++)
                    past.Append(txts[i]);
                nexts = new StringBuilder(messagetxt.Text.Length);
                for (uint j = ls.position; j < txts.Length; j++)
                    nexts.Append(txts[j]);
                ls.OnPlay = true;
            }
            messagetxt.Text = null;
            uint pos = 0;
            int howlong = ls.interval;
            signaller.BeginService();
            while(ls.position<txts.Length)
            {
                pos = ls.position;
                nexts.Remove(0, 1);    
                processed.FormattedText.Spans[0].Text = past.ToString();
                processed.FormattedText.Spans[1].Text = txts[pos].ToString();
                processed.FormattedText.Spans[2].Text = nexts.ToString();                                
                past.Append(txts[pos]);
                if (ls==null)
                    return;
                
                if (LetterSettings.letters.ContainsKey(txts[pos]))
                {
                    foreach (byte b in LetterSettings.letters[txts[pos]])
                    {
                        await signaller.flash(howlong * (b == 1 ? 3 : 1), beeper.IsChecked);
                        await Task.Delay(howlong);
                    }
                    await Task.Delay(howlong * 3);
                }
                else
                {
                    if(txts[pos]==' ' && pos>0 && txts[pos -1]!=' ')
                        await Task.Delay(howlong * 7);
                } 
                if (ls == null || !ls.OnPlay)
                    return;
                ls.position++;
            }
            pause_Clicked(stop, e);
        }

        private void pause_Clicked(object sender, EventArgs e)
        {
            InstantiatePlay();
            chooseVel.IsEnabled = true; 
            if (ls == null)
                return;
            ls.OnPlay = false;
            signaller.EndService();
            messagetxt.Text = ls.Txt;
            if (sender == stop)
            {
                ls = null;
                foreach(Span sp in processed.FormattedText.Spans)
                    sp.Text = null;
            }
        }

        private void beeper_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if((sender as CheckBox).IsChecked && signaller.AlreadyMusic())
                (sender as CheckBox).IsChecked = false;           
        }
    }
}
