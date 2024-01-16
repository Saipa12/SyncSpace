using SyncSpace.Database.Entities;

namespace SyncSpace.Client.Services
{
	public interface IUserInformation
	{
		User User { get; set; }
		string ConnectionString { get; set; }
	}
}