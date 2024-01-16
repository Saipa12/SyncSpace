using Microsoft.JSInterop;
using SyncSpace.Map.JsInterops.Base;

namespace SyncSpace.Map.JsInterops.Maps
{
	internal interface IMapJsInterop : IBaseJsInterop
	{
		ValueTask<IJSObjectReference> Initialize(MapOptions mapOptions);
	}
}