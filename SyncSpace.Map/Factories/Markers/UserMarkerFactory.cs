using Microsoft.JSInterop;
using SyncSpace.Client.Models.Markers;
using SyncSpace.Database.Entities;
using SyncSpace.Map.JsInterops.Events;

namespace SyncSpace.Map.Factories.Markers;

public class UserMarkerFactory : IUserMarkerFactory
{
	private const string Identifier = "L.userMarker";

	private readonly IJSRuntime _JsRuntime;
	private readonly IEventedJsInterop _EventedJsInterop;

	public UserMarkerFactory(IJSRuntime jsRuntime, IEventedJsInterop eventedJsInterop) => (_JsRuntime, _EventedJsInterop) = (jsRuntime, eventedJsInterop);

	public async Task<UserMarker> Create(LatLng latLng, User user)
	{
		var jsReference = await _JsRuntime.InvokeAsync<IJSObjectReference>(Identifier, latLng);
		var userMarker = new UserMarker(jsReference, _EventedJsInterop, user);
		await userMarker.FillAsync();
		return userMarker;
	}

	public async Task<UserMarker> Create(LatLng latLng, MarkerOptions options, User user)
	{
		var jsReference = await _JsRuntime.InvokeAsync<IJSObjectReference>(Identifier, latLng, options);
		var userMarker = new UserMarker(jsReference, _EventedJsInterop, user);
		await userMarker.FillAsync();
		return userMarker;
	}

	public async Task<UserMarker> CreateAndAddToMap(LatLng latLng, Map map, User user)
	{
		var marker = await Create(latLng, user);
		await marker.AddTo(map);
		return marker;
	}

	public async Task<UserMarker> CreateAndAddToMap(LatLng latLng, Map map, MarkerOptions options, User user)
	{
		var marker = await Create(latLng, options, user);
		await marker.AddTo(map);
		return marker;
	}
}