namespace SyncSpace.Client.Services
{
	public class MapLocation : IMapLocation
	{
		public bool IsUse { get; set; } = false;
		public double Latitude { get; set; }

		public double Longitude { get; set; }
	}
}