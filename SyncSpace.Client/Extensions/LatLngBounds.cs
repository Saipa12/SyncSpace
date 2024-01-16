using SyncSpace.Map;

namespace SyncSpace.Client.Extensions
{
	internal class LatLngBounds
	{
		private LatLng latLng1 { get; set; }
		private LatLng latLng2 { get; set; }


		public LatLngBounds(LatLng latLng1, LatLng latLng2)
		{
			this.latLng1 = latLng1;
			this.latLng2 = latLng2;
		}
	}
}
