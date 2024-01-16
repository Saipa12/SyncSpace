using Microsoft.EntityFrameworkCore;
using SyncSpace.Core.Controls;
using SyncSpace.Database.Contexts;
using SyncSpace.Database.Entities;

//using Microsoft.Maui.WebView.Core;
namespace SyncSpace.Client.Pages
{
    public partial class ChangeUserInfo
    {
        public QrCode _QrCode;
        private bool _IsQrCodeOpen = false;

        // private Microsoft.Maui.Controls.Model qrModalRef;

		protected override async Task OnInitializedAsync()
		{
			await Task.Delay(500);
			CreateQrCode(AuthorizedUser.User.Id);
		}

		public void OpenMap()
		{
			Navigation.NavigateTo("MainMap");
		}

        public async void SaveChangeUserInfo()
        {
            // AuthorizedUser.User.BirthDay = Convert.ToDateTime(BirthDay);
            DatabaseContext model = new(AuthorizedUser.ConnectionString);
            var user = await model.Users.Include(x => x.Avatar).FirstOrDefaultAsync(x => x.Email == AuthorizedUser.User.Email);
            user = AuthorizedUser.User;
            model.SaveChanges();
        }

        public void ChangeQrCodeState()
        {
            _IsQrCodeOpen = !_IsQrCodeOpen;
        }

		public async void CreateQrCode(long userId)
		{
			_QrCode.SetArgs(50, 50, userId.ToString());
			await _QrCode.Initialize();
		}
	}
}