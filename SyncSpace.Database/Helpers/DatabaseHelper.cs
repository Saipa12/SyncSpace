using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using SyncSpace.Database.Contexts;
using SyncSpace.Database.Exceptions;

namespace SyncSpace.Database.Helpers;

public static class DatabaseHelper
{
	public static async Task UpdateDatabase(string connectionString, string targetMigration = null)
	{
		using var context = new DatabaseContext(connectionString);
		targetMigration ??= GetLocalMigrations().Last();
		context.GetInfrastructure().GetService<IMigrator>().Migrate(targetMigration);
		if (!context.Database.GetPendingMigrations().Any())
			await new DatabaseInitializer().InitializeDatabaseAsync(context);
	}

	public static async Task<bool> TestDatabaseConnectionAsync(string connectionString, bool throwOnError = false, CancellationToken? cancellationToken = null)
	{
		using var context = new DatabaseContext(connectionString);
		try
		{
			using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
			using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, cancellationToken.GetValueOrDefault());
			await context.Database.GetDbConnection().OpenAsync(linkedCts.Token);
			return true;
		}
		catch
		{
			if (throwOnError)
				throw new DatabaseConnectionException();
			return false;
		}
	}

	public static IEnumerable<string> GetLocalMigrations()
	{
		using var context = new DatabaseContext();
		return context.Database.GetMigrations();
	}
}