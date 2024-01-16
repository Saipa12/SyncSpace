using Microsoft.JSInterop;
using SyncSpace.Map.JsInterops.Events;

namespace SyncSpace.Map.Factories.Markers;

public class MarkerFactory : IMarkerFactory
{
	private const string Identifier = "L.marker";

	private readonly IJSRuntime _JsRuntime;
	private readonly IEventedJsInterop _EventedJsInterop;

	public MarkerFactory(IJSRuntime jsRuntime, IEventedJsInterop eventedJsInterop) => (_JsRuntime, _EventedJsInterop) = (jsRuntime, eventedJsInterop);

	public async Task<Marker> Create(LatLng latLng)
	{
		var jsReference = await _JsRuntime.InvokeAsync<IJSObjectReference>(Identifier, latLng);
		return new Marker(jsReference, _EventedJsInterop);
	}

	public async Task<Marker> Create(LatLng latLng, MarkerOptions options)
	{
		var jsReference = await _JsRuntime.InvokeAsync<IJSObjectReference>(Identifier, latLng, options);
		return new Marker(jsReference, _EventedJsInterop);
	}

	public async Task<Marker> CreateAndAddToMap(LatLng latLng, Map map)
	{
		var marker = await Create(latLng);
		await marker.AddTo(map);
		return marker;
	}

	public async Task<Marker> CreateAndAddToMap(LatLng latLng, Map map, MarkerOptions options)
	{
		var marker = await Create(latLng, options);
		await marker.AddTo(map);
		return marker;
	}
}