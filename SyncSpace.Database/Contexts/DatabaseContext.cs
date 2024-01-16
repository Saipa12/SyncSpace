using Microsoft.EntityFrameworkCore;
using SyncSpace.Database.Entities;
using SyncSpace.Database.Enums;

namespace SyncSpace.Database.Contexts;

public class DatabaseContext : DbContext
{
	public DbSet<User> Users { get; set; } = null!;
	public DbSet<Avatar> Avatars { get; set; } = null!;
	public DbSet<Location> Locations { get; set; } = null!;
	public DbSet<Area> Areas { get; set; } = null!;
	public DbSet<Relationship> Relationships { get; set; } = null!;
	public DbSet<RelationshipStatus> RelationshipStatuses { get; set; } = null!;

	public DatabaseContext(string connectionString) => Database.SetConnectionString(connectionString);

	public DatabaseContext(DbContextOptions options) : base(options)
	{
	}

	public DatabaseContext()
	{
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=SyncSpace;Username=postgres;Password=root");

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>().HasAlternateKey(x => new { x.MacAddress, x.Email });

		modelBuilder.Entity<User>().Property(x => x.Id).UseIdentityAlwaysColumn();
		modelBuilder.Entity<Area>().Property(x => x.Id).UseIdentityAlwaysColumn();
		modelBuilder.Entity<Relationship>().Property(x => x.Id).UseIdentityAlwaysColumn();
		modelBuilder.Entity<RelationshipStatus>().Property(x => x.Id).UseIdentityAlwaysColumn();
		modelBuilder.Entity<Avatar>().Property(x => x.Id).UseIdentityAlwaysColumn();
		modelBuilder.Entity<Location>().Property(x => x.Id).UseIdentityAlwaysColumn();
		modelBuilder.Entity<RelationshipStatus>().Property(x => x.Name).HasMaxLength(50)
			.HasConversion(x => x.ToString(), x => (RelationType)Enum.Parse(typeof(RelationType), x)).IsUnicode(false);

		modelBuilder.Entity<User>().HasMany(x => x.Areas).WithOne(x => x.User);
		modelBuilder.Entity<User>().HasMany(x => x.SendRelationships).WithOne(x => x.Sender);
		modelBuilder.Entity<User>().HasMany(x => x.ReciveRelationships).WithOne(x => x.Receiver);
	}
}