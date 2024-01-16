using Microsoft.EntityFrameworkCore;
using SyncSpace.Database.Contexts;
using Db = SyncSpace.Database.Entities;

namespace SyncSpace.Client.Pages
{
	public partial class Chats
	{
		private static string connString = "Host=localhost;Port=5432;Database=SyncSpace;Username=postgres;Password=root";
		private static DatabaseContext model = new(connString);
		private List<Db.User> Friends = model.Users.Include(x => x.Avatar).ToList();

		public void OpenMap()
		{
			Navigation.NavigateTo("MainMap");
		}
	}
}