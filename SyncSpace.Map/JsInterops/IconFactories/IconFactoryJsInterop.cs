using Microsoft.JSInterop;
using SyncSpace.Map.JsInterops.Base;

namespace SyncSpace.Map.JsInterops.IconFactories
{
	internal class IconFactoryJsInterop : BaseJsInterop, IIconFactoryJsInterop
	{
		private static readonly string jsFilePath = $"{JsInteropConfig.BaseJsFolder}{JsInteropConfig.IconFactoryFile}";
		private const string createDefaultIcon = "createDefaultIcon";

		public IconFactoryJsInterop(IJSRuntime jsRuntime) : base(jsRuntime, jsFilePath)
		{
		}

		public async ValueTask<IJSObjectReference> CreateDefaultIcon()
		{
			IJSObjectReference module = await moduleTask.Value;
			return await module.InvokeAsync<IJSObjectReference>(createDefaultIcon);
		}
	}
}