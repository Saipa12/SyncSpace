using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;

namespace SyncSpace.Client
{
	[Service]
	internal class NotificationService : Service
	{
		public override IBinder OnBind(Intent intent)
		{
			return null;
		}

		private System.Threading.Timer timer;
		NotificationSender sender;

		[return: GeneratedEnum]
		public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
		{
			sender = new NotificationSender();
			//sender.SendNotification(this, "Hell", "From hell times...", null);
			int temp = 0;
			//StartForeground(800, sender.getNotification());
			timer = new System.Threading.Timer(_ =>
			{
				temp++;
				GetUpdates(temp);
			}, null, 1000, 1000);

			return StartCommandResult.Sticky;
		}

		private void GetUpdates(int a)
		{
			sender.SendNotification(this, "Hell", "From hell " + a + " times...", null);
		}
	}
}