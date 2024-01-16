using Microsoft.JSInterop;
using SyncSpace.Map.JsInterops.Events;

namespace SyncSpace.Map.Models.Polygons;

public class EditablePolygon : Polygon
{
	private const string RemoveLatLngJsFunction = "removeLatLng";
	private const string EnableDefaultModeJsFunction = "enableDefaultMode";
	private const string EnableAddModeJsFunction = "enableAddMode";
	private const string EnableDeleteModeJsFunction = "enableDeleteMode";
	internal EditablePolygon(IJSObjectReference jsReference, IEventedJsInterop eventedJsInterop) : base(jsReference, eventedJsInterop)
	{
	}

	public async Task RemoveLatLng(int index) => await JsReference.InvokeVoidAsync(RemoveLatLngJsFunction, index);
	public async Task EnableDefaultMode() => await JsReference.InvokeVoidAsync(EnableDefaultModeJsFunction);
	public async Task EnableAddMode() => await JsReference.InvokeVoidAsync(EnableAddModeJsFunction);
	public async Task EnableDeleteMode() => await JsReference.InvokeVoidAsync(EnableDeleteModeJsFunction);
}