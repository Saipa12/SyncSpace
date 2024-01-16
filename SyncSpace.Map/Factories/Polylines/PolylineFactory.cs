using Microsoft.JSInterop;
using SyncSpace.Map.JsInterops.Events;
using SyncSpace.Map.Models.Polylines;

namespace SyncSpace.Map
{
	internal class PolylineFactory : IPolylineFactory
	{
		private const string create = "L.polyline";
		private readonly IJSRuntime jsRuntime;
		private readonly IEventedJsInterop eventedJsInterop;

		public PolylineFactory(
			IJSRuntime jsRuntime,
			IEventedJsInterop eventedJsInterop)
		{
			this.jsRuntime = jsRuntime;
			this.eventedJsInterop = eventedJsInterop;
		}

		public async Task<Polyline> Create(IEnumerable<LatLng> latLngs)
		{
			IJSObjectReference jsReference = await this.jsRuntime.InvokeAsync<IJSObjectReference>(create, latLngs);
			return new Polyline(jsReference, this.eventedJsInterop);
		}

		public async Task<Polyline> Create(IEnumerable<LatLng> latLngs, PolylineOptions options)
		{
			IJSObjectReference jsReference = await this.jsRuntime.InvokeAsync<IJSObjectReference>(create, latLngs, options);
			return new Polyline(jsReference, this.eventedJsInterop);
		}

		public async Task<Polyline> CreateAndAddToMap(IEnumerable<LatLng> latLngs, Map map)
		{
			Polyline polyline = await this.Create(latLngs);
			await polyline.AddTo(map);
			return polyline;
		}

		public async Task<Polyline> CreateAndAddToMap(IEnumerable<LatLng> latLngs, Map map, PolylineOptions options)
		{
			Polyline polyline = await this.Create(latLngs, options);
			await polyline.AddTo(map);
			return polyline;
		}
	}
}