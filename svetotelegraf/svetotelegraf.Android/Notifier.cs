using Android.Support.V4.App;
using Android.OS;
using Android.App;
using Android.Content;
using Android.Content.Res;
using System;
using Android.Support.V4.Content;

namespace svetotelegraf.Droid
{
    static class Notifier
    {
        private static bool channelexists = false;
        public const int notifId = 0;
        static public Notification Publish(Context ctnx)
        {
            string CHANNELLID = "okwtf";
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O) 
            {
                var manager = (NotificationManager)ctnx.GetSystemService(Context.NotificationService);
                if(!channelexists)
                    CreateNotificationChannel(ref ctnx, ref manager, CHANNELLID);

                Intent Pintent = new Intent(AtsralReceiver.Filter);
                Pintent.PutExtra("what do", "pause");
                PendingIntent pi = PendingIntent.GetBroadcast(ctnx, 4, Pintent, default);

                Intent Sintent = new Intent(AtsralReceiver.Filter);
                Pintent.PutExtra("what do", "stop");
                PendingIntent pi2 = PendingIntent.GetBroadcast(ctnx, 5, Sintent, default);

                NotificationCompat.Builder builder = new NotificationCompat.Builder(ctnx, CHANNELLID)
                    .SetAutoCancel(false).SetContentTitle("Происходит ядерный бом-бом").SetContentText($"в канале {CHANNELLID}").SetProgress(100, 1, true).SetSmallIcon(Resource.Drawable.lantern)
                    .AddAction(Resource.Drawable.abc_cab_background_top_material, "pause", pi)
                    .AddAction(Resource.Drawable.abc_cab_background_top_material, "stop", pi2);
                    
                return builder.Build();
            }
            return null;
        }

        static public void Remove(ref Context ctnx) 
        {
            var manager = (NotificationManager)ctnx.GetSystemService(Context.NotificationService);
            manager.Cancel(notifId);
        }
        static private void CreateNotificationChannel(ref Context ctnx, ref NotificationManager manger, string chid)
        {
            NotificationChannel channel = new NotificationChannel(chid, "bonk", NotificationImportance.High);        
            manger.CreateNotificationChannel(channel);
            channelexists = true;
        }
    }
}
