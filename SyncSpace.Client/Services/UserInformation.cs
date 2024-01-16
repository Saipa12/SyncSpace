using Db = SyncSpace.Database.Entities;

namespace SyncSpace.Client.Services;

public class UserInformation : IUserInformation
{
    public Db.User User { get; set; }
    public string ConnectionString { get; set; } = "Host=localhost;Port=5432;Database=SyncSpace;Username=postgres;Password=1";
}