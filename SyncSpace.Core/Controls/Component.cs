using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace SyncSpace.Core.Controls;

public abstract class Component : ComponentBase
{
	private readonly string _JSClassName;

	protected IJSObjectReference JSReference;

	protected object[] Args;

	[Inject]
	public IJSRuntime JSRuntime { get; set; }

	[Parameter]
	public string Class { get; set; }

	[Parameter]
	public string Style { get; set; }

	public string Identifier { get; set; }

	protected Component(string jsClassName)
	{
		_JSClassName = jsClassName;
		Identifier = Guid.NewGuid().ToString();
	}

	protected Component(IJSObjectReference jsReference) => JSReference = jsReference;

	public async Task Initialize()
	{
		try
		{
			JSReference = await JSRuntime.InvokeAsync<IJSObjectReference>($"{_JSClassName}");
			await JSReference.InvokeVoidAsync("initialize", Identifier, Args);
		}
		catch { }
	}

	public async Task InitializeDeprecated() => await JSReference.InvokeVoidAsync("initialize");
}