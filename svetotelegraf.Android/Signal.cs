using Android.Content;
using System;
using Android.Media;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Android.Widget;

[assembly: Dependency(typeof(svetotelegraf.Droid.Signal))]
namespace svetotelegraf.Droid
{
    class Signal : ISignal
    {       
        private Intent servicToObject;
        public void BeginService(string headl) 
        {
            MessagingCenter.Subscribe<AtsralReceiver>(this, "pause", PassMessageToMainpage);
            MessagingCenter.Subscribe<AtsralReceiver>(this, "stop", PassMessageToMainpage);
            MessagingCenter.Subscribe<AtsralReceiver>(this, "play", PassMessageToMainpage);

            MessagingCenter.Subscribe<NoPalevoListener>(this, NoPalevoListener.CallBegan, PassMessageToMainpage);
            MessagingCenter.Subscribe<NoPalevoListener>(this, NoPalevoListener.CallEnded, PassMessageToMainpage);

            AM = (AudioManager)MainActivity.Current.GetSystemService(Context.AudioService);
            servicToObject = new Intent(MainActivity.Current, typeof(FFService));
            servicToObject.PutExtra("headline", headl);
            MainActivity.Current.StartService(servicToObject);            
        }

        public void ShowServicePaused()
        {
            Notifier.JustNotify_Wrapper(Notifier.Publish(MainActivity.Current, true));
        }

        private void PassMessageToMainpage(IСanCarryMessages sender) 
        {
            MessagingCenter.Send<ISignal>(this, sender.CurrentMessage.ToUpper());
            //су ка в реализациях в производных классах в случае send всегда обобщаем что тип то интерфейса!! 
            //и ты, тов. читающий, тоже запомни, а то мало ли :P
        }

        public void ReportProgress(float progress)
        {
            Notifier.ChangeProgress(progress);
        }

        private AudioManager AM;

        public bool OtherMusicExists { 
            get {
                if (AM == null)
                    AM = (AudioManager)MainActivity.Current.GetSystemService(Context.AudioService);
                return AM.IsMusicActive;
            }
        }

        public void EndService()
        {
            MessagingCenter.Unsubscribe<AtsralReceiver>(this, "play");
            MessagingCenter.Unsubscribe<AtsralReceiver>(this, "pause");
            MessagingCenter.Unsubscribe<AtsralReceiver>(this, "stop");
            MessagingCenter.Unsubscribe<NoPalevoListener>(this, NoPalevoListener.CallBegan);
            MessagingCenter.Unsubscribe<NoPalevoListener>(this, NoPalevoListener.CallEnded);


            MainActivity.Current.StopService(servicToObject);
            Notifier.clear_resources();
            servicToObject = null;
        }
        
        private MediaPlayer mlp;   
        public async Task flash(int len, bool sound)
        {
            if (sound && mlp == null) { 
                mlp = MediaPlayer.Create(MainActivity.Current, Resource.Raw.mat_beep_1_sec);
                mlp.Looping = true;
            }                
            Flashlight.TurnOnAsync();
            if(sound)
                mlp.Start();
            await Task.Delay(len);
            if(sound)
                mlp.Pause();
            Flashlight.TurnOffAsync();
        }

        public void ShowBottomPopup(string text)
        {
            Toast.MakeText(MainActivity.Current, text, ToastLength.Short).Show();
        }
    }
}