using Microsoft.JSInterop;
using SyncSpace.Map.JsInterops.Events;

namespace SyncSpace.Map.Models.Polylines;

public class Polyline : Path
{
	private const string ToGeoJSONJsFunction = "toGeoJSON";
	private const string SetLatLngsJsFunction = "setLatLngs";
	private const string GetLatLngsJsFunction = "getLatLngs";
	private const string IsEmptyJsFunction = "isEmpty";
	private const string GetCenterJsFunction = "getCenter";
	private const string AddLatLngJsFunction = "addLatLng";
	private const string GetBoundsJsFunction = "getBounds";

	internal Polyline(IJSObjectReference jsReference, IEventedJsInterop eventedJsInterop)
	{
		EventedJsInterop = eventedJsInterop;
		JsReference = jsReference;
	}

	public async Task<object> ToGeoJSON()
	{
		return await this.JsReference.InvokeAsync<object>(ToGeoJSONJsFunction);
	}

	public async Task<Polyline> SetLatLngs(IEnumerable<LatLng> latLngs)
	{
		await this.JsReference.InvokeAsync<IJSObjectReference>(SetLatLngsJsFunction, latLngs);
		return this;
	}

	public async Task<IJSObjectReference> GetBounds() => await JsReference.InvokeAsync<IJSObjectReference>(GetBoundsJsFunction);


	public async Task<LatLng[]> GetLatLngs() => await JsReference.InvokeAsync<LatLng[]>(GetLatLngsJsFunction);


	public async Task<bool> IsEmpty() => await JsReference.InvokeAsync<bool>(IsEmptyJsFunction);

	public async Task<LatLng> GetCenter()
	{
		return await this.JsReference.InvokeAsync<LatLng>(GetCenterJsFunction);
	}

	public async Task<Polyline> AddLatLng(LatLng latLng)
	{
		await this.JsReference.InvokeAsync<IJSObjectReference>(AddLatLngJsFunction, latLng);
		return this;
	}

	public async Task<Polyline> AddLatLng(LatLng latLng, IEnumerable<LatLng> latLngs)
	{
		await this.JsReference.InvokeAsync<IJSObjectReference>(AddLatLngJsFunction, latLng, latLngs);
		return this;
	}
}
