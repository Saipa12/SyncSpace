using Microsoft.JSInterop;
using SyncSpace.Map.JsInterops.Events;

namespace SyncSpace.Map.Models.Markers;

public class VertexMarker : Marker
{
	public VertexMarker(IJSObjectReference jsReference, IEventedJsInterop eventedJsInterop) : base(jsReference, eventedJsInterop)
	{
	}
}