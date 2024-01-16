using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.App;
using Intent = Android.Content.Intent;

namespace SyncSpace.Client
{
	[Application]
	public class MainApplication : MauiApplication
	{
		public MainApplication(IntPtr handle, JniHandleOwnership ownership)
			: base(handle, ownership)
		{
		}

		protected override MauiApp CreateMauiApp()
		{
			CreateNotification("Test notification", "Updates getter enabled");

			//if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
			//StartForegroundService(new Intent(this, typeof(NotificationService)));
			//else
			//StartService(new Intent(this, typeof(NotificationService)));

			return MauiProgram.CreateMauiApp();
		}

		public void StopService()
		{
			this.StopService(new Intent(this, typeof(NotificationService)));
		}

		public static Notification notif;

		public void CreateNotification(string title, string desc)
		{
			var notifManager = GetSystemService(NotificationService) as NotificationManager;
			Notification.Builder builder;

			if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
			{
				CreateNotificationChannel(this, notifManager);

				builder = new Notification.Builder(this, "25")
					.SetSmallIcon(Android.Resource.Drawable.ButtonMinus)
					.SetContentTitle(title)
					.SetContentText(desc);
			}
			else
			{
				builder = new Notification.Builder(this)
					.SetSmallIcon(Android.Resource.Drawable.ButtonMinus)
					.SetContentTitle(title)
					.SetContentText(desc)
					.SetPriority(NotificationCompat.PriorityHigh);
			}

			notif = builder.Build();
			notif.Flags = NotificationFlags.ForegroundService;

			//notifManager.Notify(256, notif);
		}

		private void CreateNotificationChannel(MainApplication mainApplication, NotificationManager manager)
		{
			var name = "Ongoing notifications";
			var descriptionText = "Foreground service that helps us notify you about latest events";
			var importance = NotificationImportance.High;
			var channel = new NotificationChannel("25", name, importance);
			channel.Description = descriptionText;

			manager.CreateNotificationChannel(channel);
		}
	}
}