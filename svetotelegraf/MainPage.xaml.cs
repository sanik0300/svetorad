using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Essentials;
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
        /// <summary>
        /// Настройка, что можно орать даже поверх музыки
        /// </summary>
        internal static bool setting_allows_beep_always;
        /// <summary>
        /// Настройка закрытия галочки на при/отсутствие ора во время проигрывания
        /// </summary>
        internal static bool setting_locks_beeper;
        public MainPage()
        {
            InitializeComponent();
            width = this.Width;
            height = this.Height;
            chooseVel.Value = Preferences.Get("speed", (double)100);
            beeper.IsChecked = Preferences.Get("beep", false);

            signaller = DependencyService.Get<ISignal>();

            play.Clicked += prePlayClick;
            play.Clicked += play_Clicked;
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

        protected void prePlayClick(object sender, EventArgs e)
        {
            MessagingCenter.Subscribe<ISignal>(this, "ZVONBEGAN", HandleCall_orMusic_Start);
            MessagingCenter.Subscribe<ISignal>(this, "ZVONENDED", HandleCall_orMusic_End);
            MessagingCenter.Subscribe<ISignal>(this, constants.Pause, PauseMessageReceived);
            if (ls == null)
            {
                MessagingCenter.Subscribe<ISignal>(this, constants.Play, PlayMessageReceived);               
                MessagingCenter.Subscribe<ISignal>(this, constants.Stop, StopMessageReceived);
            }
        }

        private async void play_Clicked(object sender, EventArgs e)
        {
            if (messagetxt.Text == null)
                return;

            messagetxt.Text = messagetxt.Text.ToLower();//Ибо больших букав в словаре нет и не буит
            InstantiatePause();//меняем разметку на кнопки паузы
            chooseVel.IsEnabled = false;//нельзя уже выбирать скорость
            if (setting_locks_beeper)
                beeper.IsEnabled = false;//а если есть соответствующая настройка, то нельзя и менять будет ли звук
            char[] txts = messagetxt.Text.ToLower().ToCharArray();//делим текст на буквы
            StringBuilder past = new StringBuilder(messagetxt.Text.Length), nexts;//готовим пустой sb с пройденными буквами         
           
            if (ls == null) { //Если нажимаем плей с нуля
                ls = new LetterSettings(messagetxt.Text, (int)chooseVel.Value, beeper.IsChecked);
                nexts = new StringBuilder(messagetxt.Text, messagetxt.Text.Length); //nexts это буквы, что ещё не отсвечивали
            }          
            else // Если продолжаем воспроизведение после паузы
            {
                ls.position++;//переход на следующую букву
                for (int i = 0; i < ls.position; i++)
                    past.Append(txts[i]);//предыдущую складываем в пройденные
                nexts = new StringBuilder(messagetxt.Text.Length);//всё что перед ней - в предстоящие
                for (uint j = ls.position; j < txts.Length; j++)
                    nexts.Append(txts[j]);//заполняем предстоящие
                ls.OnPlay = true;//возвращаем статус воспроизведения на true
            }
            messagetxt.Text = null;//убираем чёрные буквы
            messagetxt.IsReadOnly = false;
            uint pos = 0;
            int howlong = ls.interval;
            signaller.BeginService(Preferences.Get("showtext", true) ? ls.Txt : "сигналим текст...");//ебошим уведомление

            while (ls.position < txts.Length)//До конца текста:
            {
                pos = ls.position;
                nexts.Remove(0, 1);//убираем из предстоящих ближайшую букву
                processed.FormattedText.Spans[0].Text = past.ToString();//красивый цветной текст пройденных букв
                processed.FormattedText.Spans[1].Text = txts[pos].ToString();//светимую на данный момент букву красим отдельно
                processed.FormattedText.Spans[2].Text = nexts.ToString();//ещё одним цветом показываем предстоящие буквы
                past.Append(txts[pos]);//данную букву кладём в пройденные (для следующего захода)
                if (ls == null)
                    return;

                //Проверка на возможное от/включение ора в связи с музыкой--------    
                if (!setting_allows_beep_always)
                {   
                    if (signaller.OtherMusicExists && beeper.IsEnabled) { //1) Если ВНЕЗАПНО включилась музыка, а галочка на ор ещё доступна
                        HandleCall_orMusic_Start(null);
                    }
                    else if (!signaller.OtherMusicExists && !beeper.IsEnabled && !setting_locks_beeper) {
                        //Если музыки уже нет, а галочка на ор всё ещё выключена и включать её можно
                        HandleCall_orMusic_End(null);
                    }
                }//----------

                if (LetterSettings.letters.ContainsKey(txts[pos]))//Если буква есть в словаре возможных
                {
                    foreach (byte b in LetterSettings.letters[txts[pos]])//Перебираем точки и тире в её составе
                    {
                        await signaller.flash(howlong * (b == 1 ? 3 : 1), beeper.IsChecked);
                        await Task.Delay(howlong);
                    }
                    await Task.Delay(howlong * 3);
                }
                else // Если это пробел или что-то не то
                {
                    if (txts[pos] == ' ' && pos > 0 && txts[pos - 1] != ' ')
                        await Task.Delay(howlong * 7);
                }
                signaller.ReportProgress((float)(pos + 1) / txts.Length);


                if (ls == null || !ls.OnPlay)//Если воспроизведение остановили...
                    return;
                ls.position++; //А если нет, то переходим на следующую букву)
            }
            pause_Clicked(stop, e);//в любом случае нужно нажать паузу, хотя бы ради изменения кнопок
        }
        
        //----маленькие методы принятия каждого вида сообщения
        private void PlayMessageReceived(ISignal sender) {
            prePlayClick(null, null);
            play_Clicked(null, null); 
        }
        private void PauseMessageReceived(ISignal sender) { pause_Clicked(pause, null); }
        private void StopMessageReceived(ISignal sender) { pause_Clicked(stop, null); }

        private void HandleCall_orMusic_Start(ISignal sender)
        {
            beeper.IsChecked = beeper.IsEnabled = false;
        }
        private void HandleCall_orMusic_End(ISignal sender)
        {
            if(setting_allows_beep_always || !signaller.OtherMusicExists) {
                beeper.IsEnabled = true;
                if (ls.OriginallyBeeps)
                    beeper.IsChecked = true;
            }
        }
        //----------------

        private void pause_Clicked(object sender, EventArgs e)
        {
            InstantiatePlay();
            chooseVel.IsEnabled = true;
            beeper.IsEnabled = true;
            if (ls == null)
                return;
            ls.OnPlay = false;
            messagetxt.Text = ls.Txt;
            messagetxt.IsReadOnly = true;
            MessagingCenter.Unsubscribe<ISignal>(this, "ZVONBEGAN");
            MessagingCenter.Unsubscribe<ISignal>(this, "ZVONENDED");
            MessagingCenter.Unsubscribe<ISignal>(this, constants.Pause);
            if (sender == stop)
            {
                MessagingCenter.Unsubscribe<ISignal>(this, constants.Play);
                MessagingCenter.Unsubscribe<ISignal>(this, constants.Stop);
                signaller.EndService();
            }
            else
            {
                signaller.ShowServicePaused();
                return;//Полная остановка со сбросом, если
            }
            
            ls = null;
            messagetxt.IsReadOnly = false;
            foreach (Span sp in processed.FormattedText.Spans)
                sp.Text = null;
        }

        private bool really_fire_popup = true;//эт потому что событие срабатывает 2 раза
        private void beeper_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (signaller == null)//это может быть только во время инициализации страницы
                return;
            //Можно менять, если можно орать в любом случае, или же музыки нет
            if (setting_allows_beep_always || (!signaller.OtherMusicExists))
            {
                if (ls != null) { ls.Un_Mute(beeper.IsChecked); }
            }
            else
            {
                (sender as CheckBox).IsChecked = false;//вот на этом моменте
                if(really_fire_popup) { 
                        signaller.ShowBottomPopup("нельзя включать сейчас"); }                     
                really_fire_popup = !really_fire_popup;
            }            
        }

        protected override void OnDisappearing()
        {
            Preferences.Set("speed", chooseVel.Value);
            Preferences.Set("beep", beeper.IsChecked);
            base.OnDisappearing();
        }
    }
}
