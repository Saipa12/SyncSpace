using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;
using SyncSpace.Database.Contexts;
using SyncSpace.Database.Entities;

namespace SyncSpace.Client.Pages
{
	public partial class Authorization
	{
		private string _Email;
		private string _Password;
		public bool _IsRememberPassword { get; set; } = false;

		private async void SendRequest()
		{
			var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			string macAddress = networkInterfaces[0].Id.ToString();
			int chargeLevel = (int)Math.Floor(Battery.ChargeLevel);
			bool bataryState = Battery.State == BatteryState.Charging;

			// var connString = "Host=localhost;Port=5432;Database=mydb1;Username=postgres;Password=1234";
			var connString = $"Host=localhost;Port=5432;Database=SyncSpace;Username={_Email};Password={_Password}";
			var model = new DatabaseContext(AuthorizedUser.ConnectionString);

			if (model.Database.CanConnect())
			{
				//using (var mod = new DatabaseContext("Host=localhost;Port=5432;Database=mydb1;Username=postgres;Password=2412"))
				//{
				//    var me = await mod.Users.ToListAsync();
				AuthorizedUser.User = await model.Users.Include(x => x.Avatar).FirstOrDefaultAsync(x => x.Email == _Email);
				//}
				SavePreferences();
			}
			else
			{
				AuthorizedUser.User = new User()
				{
					Email = _Email,
					ChargeLevel = chargeLevel,
					BatteryState = bataryState,
					MacAddress = macAddress
				};

				model.SaveChanges();
			}
			Navigation.NavigateTo("MainMap");
		}

		protected override void OnInitialized()
		{
			var connString = "Host=localhost;Port=5432;Database=SyncSpace;Username=postgres;Password=root";
			_Email = "spipa@gmail.com";
			//var connString = "Host=localhost;Port=5432;Database=mydb1;Username=postgres;Password=1234";
			//var connString = $"Host=localhost;Port=5432;Database=postgres;Username={_Email};Password={_Password}";

			var model = new DatabaseContext(connString);
			if (model.Database.CanConnect())
			{
				AuthorizedUser.User = model.Users.Include(x => x.Avatar).FirstOrDefault(x => x.Email == _Email);

				Navigation.NavigateTo("MainMap");
			}
			//DeletePreferences();
			else
			{
				if (Preferences.Default.ContainsKey("UserName"))
				{
					_Email = Preferences.Default.Get<string>("UserName", "Saipa");
				}
				if (Preferences.Default.ContainsKey("Password"))
				{
					_Password = Preferences.Default.Get<string>("Password", "Saipa");
				}
			}
			SendRequest();
		}

		public void SavePreferences()
		{
			if (_IsRememberPassword)
			{
				Preferences.Default.Set("UserName", _Email);
				Preferences.Default.Set("Password", _Password);
			}
		}

		private void DeletePreferences()
		{
			_Email = "Saipa";
			_Password = "Password";
			Preferences.Default.Remove("UserName");
			Preferences.Default.Remove("Password");
		}
	}
}