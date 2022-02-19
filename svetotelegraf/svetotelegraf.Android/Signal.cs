using Android.Content;
using System;
using Android.Media;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(svetotelegraf.Droid.Signal))]
namespace svetotelegraf.Droid
{
    class Signal : ISignal
    {
        private static Context _context;
        public static Context Wtfcntxt { 
            get { return _context; } 
            set { if (_context == null)
                    _context = value; } 
        }

        public void SendNotifcatiom() 
        {
            Notifier.Publish(ref _context);
        }

        public void RemoveNotification() {
            Notifier.Remove(ref _context);
        }
        
        private MediaPlayer mlp;   
        public async Task flash(int len, bool sound)
        {
            if (sound && mlp == null) { 
                mlp = MediaPlayer.Create(_context, Resource.Raw.mat_beep_1_sec);
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