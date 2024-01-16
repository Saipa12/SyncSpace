using Microsoft.JSInterop;
using SyncSpace.Map.JsInterops.Events;
using SyncSpace.Map.Models.Markers;

namespace SyncSpace.Map.Factories.Markers;

public class VertexMarkerFactory : IVertexMarkerFactory
{
	private const string Identifier = "L.vertexMarker";

	private readonly IJSRuntime _JsRuntime;
	private readonly IEventedJsInterop _EventedJsInterop;

	public VertexMarkerFactory(IJSRuntime jsRuntime, IEventedJsInterop eventedJsInterop) => (_JsRuntime, _EventedJsInterop) = (jsRuntime, eventedJsInterop);

	public async Task<VertexMarker> Create(LatLng latLng)
	{
		var jsReference = await _JsRuntime.InvokeAsync<IJSObjectReference>(Identifier, latLng);
		return new VertexMarker(jsReference, _EventedJsInterop);
	}

	public async Task<VertexMarker> Create(LatLng latLng, MarkerOptions options)
	{
		var jsReference = await _JsRuntime.InvokeAsync<IJSObjectReference>(Identifier, latLng, options);
		return new VertexMarker(jsReference, _EventedJsInterop);
	}

	public async Task<VertexMarker> CreateAndAddToMap(LatLng latLng, Map map)
	{
		var marker = await Create(latLng);
		await marker.AddTo(map);
		return marker;
	}

	public async Task<VertexMarker> CreateAndAddToMap(LatLng latLng, Map map, MarkerOptions options)
	{
		var marker = await Create(latLng, options);
		await marker.AddTo(map);
		return marker;
	}
}