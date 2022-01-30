using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(svetotelegraf.Droid.Signal))]
namespace svetotelegraf.Droid
{
    class Signal : ISignal
    {
        public void BEEP()
        {
            throw new System.NotImplementedException();
        }

        public async Task flash(int len)
        {
            Flashlight.TurnOnAsync();
            await Task.Delay(len);
            Flashlight.TurnOffAsync();
        }
    }
}