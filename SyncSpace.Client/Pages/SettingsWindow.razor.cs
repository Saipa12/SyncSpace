using Microsoft.AspNetCore.Components;
using SyncSpace.Client.Services;

namespace SyncSpace.Client.Pages
{
	public partial class SettingsWindow
	{
		[Inject]
		private AppPreferences AppPreferences { get; init; }

		public void OpenMap()
		{
			Navigation.NavigateTo("MainMap");
		}


		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				StateHasChanged();
			}
		}

		private void ChangeTheme(bool isDark)
		{
			AppPreferences.IsDarkTheme = isDark;
			Preferences.Default.Set("isDark", isDark);
		}
	}
}