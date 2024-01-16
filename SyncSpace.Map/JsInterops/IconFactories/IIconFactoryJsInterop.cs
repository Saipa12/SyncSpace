using Microsoft.JSInterop;

namespace SyncSpace.Map.JsInterops.IconFactories
{
	internal interface IIconFactoryJsInterop
	{
		ValueTask<IJSObjectReference> CreateDefaultIcon();
	}
}