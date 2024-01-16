using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Maui;

namespace SyncSpace.DatabaseUpdater.Windows;

public partial class MainView : ReactiveContentPage<MainViewModel>
{
	public MainView()
	{
		InitializeComponent();
		ViewModel = new MainViewModel();

		this.WhenActivated(disposables =>
		{
			this.Bind(ViewModel, vm => vm.Host, v => v.HostEntry.Text).DisposeWith(disposables);
			this.Bind(ViewModel, vm => vm.Port, v => v.PortEntry.Text).DisposeWith(disposables);
			this.Bind(ViewModel, vm => vm.Database, v => v.DatabaseEntry.Text).DisposeWith(disposables);
			this.Bind(ViewModel, vm => vm.UserName, v => v.UserNameEntry.Text).DisposeWith(disposables);
			this.Bind(ViewModel, vm => vm.Password, v => v.PasswordEntry.Text).DisposeWith(disposables);
			this.Bind(ViewModel, vm => vm.IsConfirmed, v => v.ConfirmCheckBox.IsChecked).DisposeWith(disposables);
			this.OneWayBind(ViewModel, vm => vm.Migrations, v => v.MigrationPicker.ItemsSource).DisposeWith(disposables);
			this.Bind(ViewModel, vm => vm.SelectedMigration, v => v.MigrationPicker.SelectedItem).DisposeWith(disposables);

			this.BindCommand(ViewModel, vm => vm.UpdateDatabaseCommand, v => v.UpdateDatabaseButton).DisposeWith(disposables);

			ViewModel.UpdateDatabaseCommand.Select(result => ResultLabel.Text = result).Subscribe().DisposeWith(disposables);
		});
	}
}