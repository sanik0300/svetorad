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
        public void BeginService() 
        {
            servicToObject = new Intent(MainActivity.Current, typeof(FFService));
            MainActivity.Current.StartService(servicToObject);
        }

        public bool AlreadyMusic()
        {
            AudioManager AM = (AudioManager)MainActivity.Current.GetSystemService(Context.AudioService);
            bool music = AM.IsMusicActive;
            if (music)
                Toast.MakeText(MainActivity.Current, "no beep", ToastLength.Short).Show();
            return music;
        }

        public void EndService()
        {
            MainActivity.Current.StopService(servicToObject);
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
    }
}