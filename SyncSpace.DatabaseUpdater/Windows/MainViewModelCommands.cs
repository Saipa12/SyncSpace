using System.Reactive;
using System.Reactive.Linq;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using SyncSpace.Core.Exceptions;
using SyncSpace.Core.Helpers;
using SyncSpace.Database.Contexts;
using SyncSpace.Database.Exceptions;
using SyncSpace.Database.Helpers;

namespace SyncSpace.DatabaseUpdater.Windows;

public partial class MainViewModel
{
	public ReactiveCommand<Unit, string> UpdateDatabaseCommand { get; set; }

	private void СonfigureСommands()
	{
		UpdateDatabaseCommand = ReactiveCommand.CreateFromTask(UpdateDatabase,
			this.WhenAnyValue(x => x.Host, x => x.Port, x => x.Database, x => x.UserName, x => x.Password, x => x.IsConfirmed,
			(host, port, database, username, password, isConfirmed)
			=> !string.IsNullOrWhiteSpace(host)
			&& !string.IsNullOrWhiteSpace(port)
			&& !string.IsNullOrWhiteSpace(database)
			&& !string.IsNullOrWhiteSpace(username)
			&& !string.IsNullOrWhiteSpace(password)
			&& isConfirmed));
	}

	private async Task<string> UpdateDatabase()
	{
		try
		{
			string connectionString = $"Host={Host};Port={Port};Database={Database};Username={UserName};Password={Password};";
			await InternetHelper.TestInternetConnection(true);
			await DatabaseHelper.TestDatabaseConnectionAsync(connectionString, true);
			using var context = new DatabaseContext(connectionString);

			var databaseMigration = (await context.Database.GetAppliedMigrationsAsync()).LastOrDefault();
			if (databaseMigration != SelectedMigration)
			{
				await DatabaseHelper.UpdateDatabase(connectionString, SelectedMigration);
				return $"База данных обновлена{(string.IsNullOrWhiteSpace(databaseMigration) ? string.Empty : $" c миграции «{databaseMigration}»")} до «{SelectedMigration}».";
			}
			else
			{
				return "Обновление базы данных не требуется.";
			}
		}
		catch (Exception e)
		{
			return e switch
			{
				InternetConnectionException => "Не удалось установить интернет соединение.",
				DatabaseConnectionException => "Не удалось установить соединение с базой данных. Проверьте корректность введённых данных.",
				_ => "Не удалось обновить базу данных. Ошибка: " + e.Message
			};
		}
	}
}