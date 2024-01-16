using ReactiveUI;
using SyncSpace.Database.Helpers;

namespace SyncSpace.DatabaseUpdater.Windows;

public partial class MainViewModel : ReactiveObject
{
	private string _Host;
	private string _Port;
	private string _Database;
	private string _UserName;
	private string _Password;

	private bool _IsConfirmed;

	public string Host { get => _Host; set => this.RaiseAndSetIfChanged(ref _Host, value); }
	public string Port { get => _Port; set => this.RaiseAndSetIfChanged(ref _Port, value); }
	public string Database { get => _Database; set => this.RaiseAndSetIfChanged(ref _Database, value); }
	public string UserName { get => _UserName; set => this.RaiseAndSetIfChanged(ref _UserName, value); }
	public string Password { get => _Password; set => this.RaiseAndSetIfChanged(ref _Password, value); }

	public List<string> Migrations { get; set; }
	public string SelectedMigration { get; set; }

	public bool IsConfirmed { get => _IsConfirmed; set => this.RaiseAndSetIfChanged(ref _IsConfirmed, value); }

	public MainViewModel()
	{
		Migrations = DatabaseHelper.GetLocalMigrations().ToList();
		SelectedMigration = Migrations?.LastOrDefault();
		СonfigureСommands();
	}
}