using Microsoft.JSInterop;

namespace SyncSpace.Map.JsInterops.Base
{
	internal abstract class BaseJsInterop : IAsyncDisposable, IBaseJsInterop
	{
		protected readonly Lazy<Task<IJSObjectReference>> moduleTask;

		public BaseJsInterop(IJSRuntime jsRuntime, string jsFilePath)
		{
			this.moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
			   JsInteropConfig.Import, jsFilePath).AsTask());
		}

		public async ValueTask DisposeAsync()
		{
			if (this.moduleTask.IsValueCreated)
			{
				IJSObjectReference module = await this.moduleTask.Value;
				await module.DisposeAsync();
			}
		}
	}
}