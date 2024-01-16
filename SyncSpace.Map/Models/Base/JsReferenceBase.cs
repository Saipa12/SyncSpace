using Microsoft.JSInterop;

namespace SyncSpace.Map
{
	/// <summary>
	/// An abstract class that makes it possible to call methods directly on JS objects.
	/// </summary>
	public abstract class JsReferenceBase
	{
		internal IJSObjectReference JsReference;
	}
}