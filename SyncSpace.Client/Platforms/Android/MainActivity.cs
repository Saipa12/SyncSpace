using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Provider;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Java.Lang;
using Microsoft.Maui.Platform;
using static Android.Provider.Settings;
using Uri = Android.Net.Uri;

namespace SyncSpace.Client
{
	[Activity(/*Theme = "@style/Maui.SplashTheme"*/ Theme = "@style/Maui.MainTheme.NoActionBar", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
	public class MainActivity : MauiAppCompatActivity
	{
		readonly string[] permissions = new string[]{
		Manifest.Permission.AccessCoarseLocation,
		Manifest.Permission.AccessFineLocation,
		Manifest.Permission.PostNotifications};



		protected override void OnCreate(Bundle savedInstanceState)
		{
			CheckAndRequestPermissions();
			if (!isLocationEnabled(ApplicationContext))
			{
				Intent callGPSSettingIntent = new Intent(ActionLocationSourceSettings);
				StartActivity(callGPSSettingIntent);
				Toast.MakeText(ApplicationContext, "Location services are disabled. Please enable them for this app to work.", ToastLength.Long).Show();
			}

			base.OnCreate(savedInstanceState);
		}



		public void CheckAndRequestPermissions()
		{
			List<string> listPermissionsNeeded = new();
			List<string> listPermissionsToEnableInSettings = new();

			foreach (string permission in permissions)
			{
				if (ContextCompat.CheckSelfPermission(ApplicationContext, permission) != PackageManager.CheckPermission(permission, PackageName))
					listPermissionsNeeded.Add(permission);

				if (ActivityCompat.ShouldShowRequestPermissionRationale(this.GetActivity(), permission))
					listPermissionsToEnableInSettings.Add(permission);

				if (!ActivityCompat.ShouldShowRequestPermissionRationale(this.GetActivity(), permission))
					listPermissionsToEnableInSettings.Add(permission);
			}

			if (!listPermissionsNeeded.Any())
				return;

			if (listPermissionsNeeded.Any())
				ActivityCompat.RequestPermissions(this.GetActivity(), listPermissionsNeeded.ToArray(), 11);

			if (listPermissionsToEnableInSettings.Any())
			{
				Toast.MakeText(this, "This app cannot work without Location, Notifications permissions. Please enable them in the app settings page, please.", ToastLength.Long).Show();

				Intent intent = new Intent(Settings.ActionApplicationDetailsSettings);
				Uri uri = Uri.FromParts("package", PackageName, null);
				intent.SetData(uri);
				StartActivity(intent);
			}
		}



		public static bool isLocationEnabled(Context context)
		{
			if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.P)
			{
				// This is a new method provided in API 28
				LocationManager lm = (LocationManager)context.GetSystemService(Context.LocationService);
				return lm.IsLocationEnabled;
			}
			else
			{
				// This was deprecated in API 28
				int mode = Secure.GetInt(context.ContentResolver, Secure.LocationMode, (int)SecurityLocationMode.Off);
				return (mode != (int)Secure.LocationModeOff);
			}
		}



		[Override]
		protected override void OnDestroy()
		{
			//Toast.MakeText(this, "A", ToastLength.Short).Show();
			StartService(new Intent(this, typeof(NotificationService)));
			base.OnDestroy();
		}

		protected override void OnStop()
		{
			//Toast.MakeText(this, "A", ToastLength.Short).Show();
			StartService(new Intent(this, typeof(NotificationService)));
			base.OnStop();
		}

		protected override void OnPause()
		{
			//Toast.MakeText(this, "A", ToastLength.Short).Show();
			StartService(new Intent(this, typeof(NotificationService)));
			base.OnPause();
		}

		public override void OnBackPressed()
		{
			//Toast.MakeText(this, "A", ToastLength.Short).Show();
			StartService(new Intent(this, typeof(NotificationService)));
			base.OnBackPressed();
		}
	}
}