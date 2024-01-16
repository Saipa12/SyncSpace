using Microsoft.JSInterop;

namespace SyncSpace.Core.Controls;

public class Battery : Component
{
	public Battery(IJSObjectReference jsReference) : base(jsReference)
	{
	}

	public async Task SetBatteryLevel(int level) => await JSReference.InvokeVoidAsync("setBatteryLevel", level);
}