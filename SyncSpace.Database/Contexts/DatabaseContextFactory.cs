using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SyncSpace.Database.Contexts;

public class DbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
	public DatabaseContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
		optionsBuilder.UseNpgsql();

		return new DatabaseContext(optionsBuilder.Options);
	}
}