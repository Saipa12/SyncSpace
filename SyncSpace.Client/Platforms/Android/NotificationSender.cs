using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;

namespace SyncSpace.Client
{
	[Application]
	public class NotificationSender
	{
		Context context { get; set; }
		Notification notification { get; set; }
		string title { get; set; }
		string description { get; set; }
		string icon { get; set; }

		public void SendNotification(Context context, string title, string desc, string icon)
		{
			Notification notification;
			var notifManager = context.GetSystemService("notification") as NotificationManager;
			Notification.Builder builder;

			builder = Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O
				? new Notification.Builder(context, "25").SetPriority(NotificationCompat.PriorityHigh)
				: new Notification.Builder(context);

			builder = builder
					.SetSmallIcon(Android.Resource.Drawable.ButtonMinus)
					.SetContentTitle(title)
					.SetContentText(desc)
					.SetOnlyAlertOnce(true);

			if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
				CreateNotificationChannel(notifManager);

			notification = builder.Build();
			this.notification = notification;
			notifManager.Notify(256, notification);
		}

		private void CreateNotificationChannel(NotificationManager manager)
		{
			var name = "Ongoing notifications";
			var descriptionText = "Foreground service that helps us notify you about latest events";
			var importance = NotificationImportance.High;
			var channel = new NotificationChannel("25", name, importance);
			channel.Description = descriptionText;

			manager.CreateNotificationChannel(channel);
		}

		public Notification getNotification()
		{
			return notification;
		}
	}
}