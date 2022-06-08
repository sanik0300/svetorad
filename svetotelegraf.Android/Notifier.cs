using Android.Support.V4.App;
using Android.OS;
using Android.App;
using Android.Content;
using Android.Content.Res;
using svetotelegraf;

namespace svetotelegraf.Droid
{
    static class Notifier
    {
        private static bool channelexists = false;
        /// <summary>
        /// id для уведомлений, чтоб уведомление всегда было одно
        /// </summary>
        internal const int ProperNotifId = 1;

        /// <summary>
        /// Выдаёт уведомление и изменяет его заготовку
        /// </summary>
        /// <param name="play">Определяет кнопки внизу: true - будет плей/стоп, false - пауза/стоп</param>
        /// <param name="ctnx">контекст, он же main activity</param>
        /// <param name="headline">(новый) заголовок</param>
        /// <returns></returns>
        static public Notification Publish(Context ctnx, bool play, string headline="")
        {
            Intent Pintent = new Intent(AtsralReceiver.Filter);
            Pintent.PutExtra("what do", play ? "play" : "pause");
            PendingIntent pi = PendingIntent.GetBroadcast(ctnx, 4, Pintent, PendingIntentFlags.UpdateCurrent);

            Intent Sintent = new Intent(AtsralReceiver.Filter);
            Sintent.PutExtra("what do", "stop");
            PendingIntent pi2 = PendingIntent.GetBroadcast(ctnx, 5, Sintent, default);

            if (builder == null)
            {
                builder = GiveDefaultNtf(ctnx);
            }
            if (headline != string.Empty)
                builder = builder.SetContentTitle(headline);

            builder.SetContentText(play ? AppResources.on_pause : string.Empty);

            NotificationCompat.Action aaaa1 = new NotificationCompat.Action(
                    Resource.Drawable.abc_cab_background_top_material, play ? AppResources.play_keyword : AppResources.pause_keyword, pi);
            if (builder.MActions.Count == 0)
                builder = builder.AddAction(aaaa1).AddAction(Resource.Drawable.abc_cab_background_top_material, AppResources.stop_keyword, pi2);
            else
                builder.MActions[0] = aaaa1;

            return builder.Build();
        }

        static public void JustNotify_Wrapper(Notification n)
        {
            if (manager != null)
                manager.Notify(ProperNotifId, n);
        }


        static public void ChangeProgress(float percents)
        {
            if (builder == null || manager == null)
                return;
            builder.SetProgress(100, (int)(percents * 100), false);
            manager.Notify(ProperNotifId, builder.Build());
        }

        static internal void clear_resources()
        {
            manager = null;
            builder = null;
        }

        private static NotificationManager manager;
        private static NotificationCompat.Builder builder;
        private static NotificationCompat.Builder GiveDefaultNtf(Context ctnx) 
        {          
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                string CHANNELLID = "okwtf";
                manager = (NotificationManager)ctnx.GetSystemService(Context.NotificationService);
                if (!channelexists)
                {
                    NotificationChannel channel = new NotificationChannel(CHANNELLID, "bonk", NotificationImportance.High);
                    manager.CreateNotificationChannel(channel);
                    channelexists = true;
                }
                builder = new NotificationCompat.Builder(ctnx, CHANNELLID);                   
            }
            else
            {
                builder = new NotificationCompat.Builder(ctnx);
            }
            return builder.SetAutoCancel(false).SetProgress(100, 0, false).SetSmallIcon(Resource.Drawable.lantern);
        }
    }
}
