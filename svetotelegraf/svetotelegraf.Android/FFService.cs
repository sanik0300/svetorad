using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace svetotelegraf.Droid
{
    [Service]
    class FFService : Service
    {
        AtsralReceiver receiver;
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            receiver = new AtsralReceiver();
            RegisterReceiver(receiver, new IntentFilter(AtsralReceiver.Filter));
            StartForeground(1, Notifier.Publish(MainActivity.Current));
            return base.OnStartCommand(intent, flags, startId);
        }

        public override void OnDestroy()
        {
            StopForeground(true);
            UnregisterReceiver(receiver);
            base.OnDestroy();
        }
    }
}