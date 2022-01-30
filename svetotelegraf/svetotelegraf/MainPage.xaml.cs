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
        LetterSettings ls = null;
        ISignal signaller;
        public MainPage()
        {
            InitializeComponent();
            width = this.Width;
            height = this.Height;
            signaller = DependencyService.Get<ISignal>();
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
                lil_buttons.RowDefinitions.Clear();
                lil_buttons.ColumnDefinitions.Clear();
                bool horizontal = width > height;

                if (horizontal)
                {
                    allingrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(4, GridUnitType.Star) });
                    allingrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                    allingrid.Children.Add(ui_stuff, 1, 0);
                    for (int i = 0; i < 2; i++)
                        lil_buttons.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                    lil_buttons.Children.Add(pause, 0, 0);
                    lil_buttons.Children.Add(stop, 0, 1);
                    Grid.SetRowSpan(play, 2);
                    play.HeightRequest = play.Width;
                }
                else
                {
                    allingrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(3, GridUnitType.Star) });
                    allingrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                    allingrid.Children.Add(ui_stuff, 0, 1);
                    for (int i = 0; i < 2; i++)
                        lil_buttons.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                    lil_buttons.Children.Add(pause, 0, 0);
                    lil_buttons.Children.Add(stop, 1, 0);
                    Grid.SetColumnSpan(play, 2);
                    play.WidthRequest = play.Height;
                }
                ui_stuff.Orientation = horizontal? StackOrientation.Horizontal : StackOrientation.Vertical;
                play.HorizontalOptions = horizontal ? LayoutOptions.Fill : LayoutOptions.Center;
                play.VerticalOptions = horizontal ? LayoutOptions.Center : LayoutOptions.Fill;
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
            pause.IsVisible = stop.IsVisible = true;
            play.IsVisible = chooseVel.IsEnabled = false;                   

            char[] txts = messagetxt.Text.ToCharArray();
            StringBuilder past = new StringBuilder(messagetxt.Text.Length), nexts;
            if (ls == null) { 
                ls = new LetterSettings(messagetxt.Text, (int)chooseVel.Value, false);
                nexts = new StringBuilder(messagetxt.Text, messagetxt.Text.Length);
            }
            else
            {
                for(int i =0; i<ls.position; i++)
                    past.Append(txts[i]);
                nexts = new StringBuilder(messagetxt.Text.Length);
                for (uint j = ls.position; j < txts.Length; j++)
                    nexts.Append(txts[j]);
                ls.OnPlay = true;
            }
            messagetxt.Text = null;
            while(ls.position<txts.Length)
            {             
                nexts.Remove(0, 1);    
                processed.FormattedText.Spans[0].Text = past.ToString();
                processed.FormattedText.Spans[1].Text = txts[ls.position].ToString();
                processed.FormattedText.Spans[2].Text = nexts.ToString();                                
                past.Append(txts[ls.position]);                            
                if (LetterSettings.letters.ContainsKey(txts[ls.position]))
                {
                    foreach (byte b in LetterSettings.letters[txts[ls.position]])
                    {
                        await signaller.flash(ls.interval * (b == 1 ? 3 : 1));
                        await Task.Delay(ls.interval);
                    }
                    await Task.Delay(ls.interval * 3);
                }
                else
                {
                    if(txts[ls.position]==' ' && ls.position>0 && txts[ls.position-1]!=' ')
                        await Task.Delay(ls.interval * 7);
                }                
                ls.position++;
                if (!ls.OnPlay)
                    return;               
            }
            pause_Clicked(stop, e);
        }

        private void pause_Clicked(object sender, EventArgs e)
        {
            pause.IsVisible = stop.IsVisible = false;
            play.IsVisible = chooseVel.IsEnabled = true; 
            if (ls == null)
                return;
            ls.OnPlay = false;
            messagetxt.Text = ls.Txt;
            if (sender == stop) 
                ls = null;
        }
    }
}
