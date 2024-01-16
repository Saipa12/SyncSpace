using Microsoft.JSInterop;

namespace SyncSpace.Core.Controls;

public class OnlineStatus : Component
{
	public OnlineStatus(IJSObjectReference jsReference) : base(jsReference)
	{
	}

	public async Task SetStatus(bool isOnline) => await JSReference.InvokeVoidAsync("setStatus", isOnline);
}