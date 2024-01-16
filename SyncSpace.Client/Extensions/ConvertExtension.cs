using SyncSpace.Map;
using Db = SyncSpace.Database.Entities;

namespace SyncSpace.Client.Extensions
{
	internal static class ConvertExtension
	{
		public static LatLng ToLatLng(this Db.Location location)
		{
			return new LatLng(location.Latitude, location.Longitude);
		}

		public static LatLng ToLatLng(this Location location)
		{
			return new LatLng(location.Latitude, location.Longitude);
		}
	}
}