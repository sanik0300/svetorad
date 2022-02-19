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
    [BroadcastReceiver(Enabled = true)]
    class AtsralReceiver : BroadcastReceiver
    {
        public static string Filter { get { return "pause this shit pls"; } }
        public override void OnReceive(Context context, Intent intent)
        {
            switch (intent.Flags) { }
            
        }
    }
}