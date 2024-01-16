using SyncSpace.Client.Models.Markers;
using SyncSpace.Database.Entities;

namespace SyncSpace.Map.Factories.Markers;

public interface IUserMarkerFactory
{
	Task<UserMarker> Create(LatLng latLng, User user);

	Task<UserMarker> Create(LatLng latLng, MarkerOptions options, User user);

	Task<UserMarker> CreateAndAddToMap(LatLng latLng, Map map, User user);

	Task<UserMarker> CreateAndAddToMap(LatLng latLng, Map map, MarkerOptions options, User user);
}