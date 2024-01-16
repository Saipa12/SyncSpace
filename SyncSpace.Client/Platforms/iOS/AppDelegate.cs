using Foundation;
using UserNotifications;

namespace SyncSpace.Client
{
	[Register("AppDelegate")]
	public class AppDelegate : MauiUIApplicationDelegate
	{
		protected override MauiApp CreateMauiApp()
		{
			GetPermissions();
			SendNotification();
			return MauiProgram.CreateMauiApp();
		}

		public static void GetPermissions()
		{
			var center = UNUserNotificationCenter.Current;
			center.RequestAuthorization(options: UNAuthorizationOptions.Badge, (granted, error) =>
			{
				//Error handler
			});
		}

		public static void SendNotification()
		{
			var content = new UNMutableNotificationContent();

			content.Title = "Test notification";
			content.Body = "Body";

			//var dateUtil = new NSDateComponents();
			//dateUtil.Calendar = NSCalendar.CurrentCalendar;

			//dateUtil.Weekday = 1;
			//dateUtil.Hour = 1;
			//dateUtil.Minute = 1;
			//dateUtil.Second = 1;

			var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(5, true);

			var uuidString = new NSUuid().AsString();
			var request = UNNotificationRequest.FromIdentifier(uuidString, content, trigger);

			var notificationCenter = UNUserNotificationCenter.Current;


			notificationCenter.AddNotificationRequest(request, error =>
			{
				if (error != null)
				{
					//Error handle
				}
			});


			//// create the notification
			//var notification = new UILocalNotification();

			//// set the fire date (the date time in which it will fire)
			//notification.FireDate = NSDate.FromTimeIntervalSinceNow(60);

			//// configure the alert
			//notification.AlertAction = "View Alert";
			//notification.AlertBody = "Your one minute alert has fired!";

			//// modify the badge
			//notification.ApplicationIconBadgeNumber = 1;

			//// set the sound to be the default sound
			//notification.SoundName = UILocalNotification.DefaultSoundName;

			//// schedule it
			//UIApplication.SharedApplication.ScheduleLocalNotification(notification);

		}
	}
}