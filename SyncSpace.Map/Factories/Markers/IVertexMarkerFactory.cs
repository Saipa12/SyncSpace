using SyncSpace.Map.Models.Markers;

namespace SyncSpace.Map.Factories.Markers;

public interface IVertexMarkerFactory
{
	Task<VertexMarker> Create(LatLng latLng);

	Task<VertexMarker> Create(LatLng latLng, MarkerOptions options);

	Task<VertexMarker> CreateAndAddToMap(LatLng latLng, Map map);

	Task<VertexMarker> CreateAndAddToMap(LatLng latLng, Map map, MarkerOptions options);
}