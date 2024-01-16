namespace SyncSpace.Client.Services
{
	public class AppPreferences
	{
		private bool _isDarkTheme;
		public bool IsDarkTheme
		{
			get { return _isDarkTheme; }
			set { _isDarkTheme = value; ChangeTheme?.Invoke(); }
		}

		public event Action ChangeTheme;
	}
}
