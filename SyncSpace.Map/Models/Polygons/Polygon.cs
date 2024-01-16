using Microsoft.JSInterop;
using SyncSpace.Map.JsInterops.Events;
using SyncSpace.Map.Models.Polylines;

namespace SyncSpace.Map
{
	public class Polygon : Polyline
	{
		internal Polygon(IJSObjectReference jsReference, IEventedJsInterop eventedJsInterop)
			: base(jsReference, eventedJsInterop)
		{
		}
	}
}