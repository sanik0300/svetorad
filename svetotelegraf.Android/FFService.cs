using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using Android.Telephony;

namespace svetotelegraf.Droid
{
    [Service]
    class FFService : Service
    {
        AtsralReceiver receiver;
        NoPalevoListener listener;
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            receiver = new AtsralReceiver();
            RegisterReceiver(receiver, new IntentFilter(AtsralReceiver.Filter));

            listener = new NoPalevoListener();
            ((TelephonyManager)GetSystemService("phone")).Listen(listener, PhoneStateListenerFlags.CallState);
            StartForeground(Notifier.ProperNotifId, Notifier.Publish(MainActivity.Current, false, intent.GetStringExtra("headline")));
            return base.OnStartCommand(intent, flags, startId);
        }



        public override void OnDestroy()
        {
            StopForeground(true);
            listener = null;
            UnregisterReceiver(receiver);
            base.OnDestroy();
        }
    }
}