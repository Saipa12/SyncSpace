using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncSpace.Database.Contexts;

namespace SyncSpace.Client.Pages
{
	public partial class Chat
	{
		private static string connString = "Host=localhost;Port=5432;Database=SyncSpace;Username=postgres;Password=root";

		public void OpenMap()
		{
			Navigation.NavigateTo("MainMap");
		}


		public void SendMessage()
		{
			//
		}
	}
}
