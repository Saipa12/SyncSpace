namespace SyncSpace.Map
{
	/// <summary>
	/// It is responsible for creating Icons.
	/// </summary>
	public interface IIconFactory
	{
		Task<Icon> Create(IconOptions options);

		Task<Icon> CreateDefault();
	}
}