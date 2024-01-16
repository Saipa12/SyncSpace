using SyncSpace.Map.Models.Polygons;

namespace SyncSpace.Map.Factories.Polygons;

public interface IEditablePolygonFactory
{
	Task<EditablePolygon> Create(IEnumerable<LatLng> latLngs, PolylineOptions options = null, MarkerOptions markerOptions = null);

	Task<EditablePolygon> CreateAndAddToMap(IEnumerable<LatLng> latLngs, Map map, PolylineOptions options = null, MarkerOptions markerOptions = null);
}
