using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace SyncSpace.Core.Controls;

public partial class BottomSheetView : Component
{
	//internal IJSRuntime JsReference;

	public BottomSheetView() : base("bottomSheetView")
	{
	}

	//public async Task InitExpander()
	//{
	//    //await this.JsReference.InvokeVoidAsync("initExpander");
	//    await JSReference.InvokeVoidAsync("initialize", Identifier);
	//}

	[Parameter]
	public RenderFragment ChildContent { get; set; }

	/// <summary>Открывает экспандер на заданную в процентах высоту.</summary>
	/// <param name="height">Высота экспандера в процентах.</param>
	public async Task OpenExpander(int height)
	{
		//await this.JsReference.InvokeVoidAsync("openBottomSheet", height);
		await JSReference.InvokeVoidAsync("openBottomSheet", height);
	}

	/// <summary>Закрывает экспандер.</summary>
	public async Task CloseExpander()
	{
		//await this.JsReference.InvokeVoidAsync("closeBottomSheet");
		await JSReference.InvokeVoidAsync("closeBottomSheet");
	}
}