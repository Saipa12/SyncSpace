using Microsoft.JSInterop;
using SyncSpace.Map.JsInterops.Events;
using SyncSpace.Map.Models.Polygons;

namespace SyncSpace.Map.Factories.Polygons;

public class EditablePolygonFactory : IEditablePolygonFactory
{
	private const string create = "L.editablePolygon";
	private readonly IJSRuntime jsRuntime;
	private readonly IEventedJsInterop eventedJsInterop;

	public EditablePolygonFactory(IJSRuntime jsRuntime, IEventedJsInterop eventedJsInterop)
	{
		this.jsRuntime = jsRuntime;
		this.eventedJsInterop = eventedJsInterop;
	}

	public async Task<EditablePolygon> Create(IEnumerable<LatLng> latLngs, PolylineOptions options = null, MarkerOptions markerOptions = null)
	{
		var jsReference = await jsRuntime.InvokeAsync<IJSObjectReference>(create, latLngs, options, markerOptions);
		return new EditablePolygon(jsReference, eventedJsInterop);
	}

	public async Task<EditablePolygon> CreateAndAddToMap(IEnumerable<LatLng> latLngs, Map map, PolylineOptions options = null, MarkerOptions markerOptions = null)
	{
		var polygon = await Create(latLngs, options, markerOptions);
		await polygon.AddTo(map);
		return polygon;
	}
}