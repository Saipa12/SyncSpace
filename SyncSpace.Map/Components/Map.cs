using System.Drawing;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SyncSpace.Map.JsInterops.Events;
using SyncSpace.Map.JsInterops.Maps;

namespace SyncSpace.Map
{
	public partial class Map : ComponentBase
	{
		private const string flyTo = "flyTo";
		private const string flyToBounds = "flyToBounds";
		private const string distance = "distance";
		private const string getCenter = "getCenter";
		private const string getZoom = "getZoom";
		private const string getMinZoom = "getMinZoom";
		private const string getMaxZoom = "getMaxZoom";
		private const string setView = "setView";
		private const string setZoom = "setZoom";
		private const string zoomIn = "zoomIn";
		private const string zoomOut = "zoomOut";
		private const string setZoomAround = "setZoomAround";
		private const string containerPointToLatLng = "containerPointToLatLng";

		[Inject]
		public IJSRuntime JsRuntime { get; set; }

		[Inject]
		internal IMapJsInterop MapJsInterop { get; set; }

		[Inject]
		internal IEventedJsInterop EventedJsInterop { get; set; }

		internal MapEvented MapEvented { get; set; }

		[Parameter]
		public MapOptions MapOptions { get; set; }

		[Parameter]
		public EventCallback AfterRender { get; set; }

		internal IJSObjectReference MapReference { get; set; }

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				MapReference = await MapJsInterop.Initialize(MapOptions);
				MapEvented = new MapEvented(MapReference, EventedJsInterop);
				await AfterRender.InvokeAsync();
			}
		}

		public async Task FlyTo(LatLng latLng, int zoom)
		{
			await MapReference.InvokeAsync<IJSObjectReference>(flyTo, latLng, zoom, 1500);
		}

		public async Task FlyTo(LatLng latLng, int zoom, int durationMs)
		{
			await MapReference.InvokeAsync<IJSObjectReference>(flyTo, latLng, zoom, durationMs);
		}

		public async Task FlyToBounds(IJSObjectReference latLng)
		{
			await MapReference.InvokeAsync<IJSObjectReference>(flyToBounds, latLng, 1500);
		}

		public async Task FlyToBounds(IJSObjectReference latLng, int durationMs)
		{
			await MapReference.InvokeAsync<IJSObjectReference>(flyToBounds, latLng, durationMs);
		}

		public async Task<LatLng> ContainerPointToLatLng(PointF point)
		{
			return await MapReference.InvokeAsync<LatLng>(containerPointToLatLng, point);
		}

		public async Task<double> DistanceBetween(LatLng firstLatLng, LatLng secondLatLng)
		{
			return await MapReference.InvokeAsync<double>(distance, firstLatLng, secondLatLng);
		}

		public async Task<LatLng> GetCenter()
		{
			return await MapReference.InvokeAsync<LatLng>(getCenter);
		}

		public async Task<float> GetZoom()
		{
			return await MapReference.InvokeAsync<float>(getZoom);
		}

		public async Task<int> GetMinZoom()
		{
			return await MapReference.InvokeAsync<int>(getMinZoom);
		}

		public async Task<int> GetMaxZoom()
		{
			return await MapReference.InvokeAsync<int>(getMaxZoom);
		}

		public async Task SetView(LatLng latLng)
		{
			await MapReference.InvokeAsync<IJSObjectReference>(setView, latLng);
		}

		public async Task SetZoom(int zoom)
		{
			await MapReference.InvokeAsync<IJSObjectReference>(setZoom, zoom);
		}

		public async Task ZoomIn(int zoomDelta)
		{
			await MapReference.InvokeAsync<IJSObjectReference>(zoomIn, zoomDelta);
		}

		public async Task ZoomOut(int zoomDelta)
		{
			await MapReference.InvokeAsync<IJSObjectReference>(zoomOut, zoomDelta);
		}

		public async Task SetZoomAround(LatLng latLng, int zoom)
		{
			await MapReference.InvokeAsync<IJSObjectReference>(setZoomAround, latLng, zoom);
		}

		public async Task OnClick(Func<MouseEvent, Task> callback)
		{
			await MapEvented.OnClick(callback);
		}

		public async Task OnDblClick(Func<MouseEvent, Task> callback)
		{
			await MapEvented.OnDblClick(callback);
		}

		public async Task OnMouseDown(Func<MouseEvent, Task> callback)
		{
			await MapEvented.OnMouseDown(callback);
		}

		public async Task OnMouseUp(Func<MouseEvent, Task> callback)
		{
			await MapEvented.OnMouseUp(callback);
		}

		public async Task OnMouseOver(Func<MouseEvent, Task> callback)
		{
			await MapEvented.OnMouseOver(callback);
		}

		public async Task OnMouseOut(Func<MouseEvent, Task> callback)
		{
			await MapEvented.OnMouseOut(callback);
		}

		public async Task OnContextMenu(Func<MouseEvent, Task> callback)
		{
			await MapEvented.OnContextMenu(callback);
		}

		public async Task Off(string eventType)
		{
			await MapEvented.Off(eventType);
		}
	}
}