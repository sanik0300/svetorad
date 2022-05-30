using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using svetotelegraf;
using Xamarin.Forms;

namespace svetotelegraf.Droid
{
    [BroadcastReceiver(Enabled = true)]
    class AtsralReceiver : BroadcastReceiver, IСanCarryMessages
    {
        public const string Filter = "pause_this_shit_pls";
        public string CurrentMessage { get; private set; }

        public override void OnReceive(Context context, Intent intent)
        {
            CurrentMessage = intent.GetStringExtra("what do");
            MessagingCenter.Send(this, CurrentMessage);
        }
    }
    class NoPalevoListener : PhoneStateListener, IСanCarryMessages
    {
        public const string CallBegan = "zvonBegan", CallEnded = "zvonEnded";

        public string CurrentMessage { get; private set; }
        private bool realcall = false;//Защита от срабатывания в первый раз при инициализации
        public override void OnCallStateChanged([GeneratedEnum] CallState state, string phoneNumber)
        {
            base.OnCallStateChanged(state, phoneNumber);
            if (realcall)
            {
                switch (state)
                {
                    case CallState.Ringing:
                        CurrentMessage = CallBegan;
                        break;
                    case CallState.Idle:
                        CurrentMessage = CallEnded;
                        break;
                }
                MessagingCenter.Send(this, CurrentMessage);
            }
            realcall = true;
        }
    }
}