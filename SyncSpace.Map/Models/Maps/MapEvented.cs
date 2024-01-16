using Microsoft.JSInterop;
using SyncSpace.Map.JsInterops.Events;

namespace SyncSpace.Map
{
	internal class MapEvented : Evented
	{
		public MapEvented(IJSObjectReference jsReference, IEventedJsInterop eventedJsInterop)
		{
			this.JsReference = jsReference;
			this.EventedJsInterop = eventedJsInterop;
		}
	}
}