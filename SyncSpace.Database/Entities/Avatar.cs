using System.ComponentModel.DataAnnotations;

namespace SyncSpace.Database.Entities;

public class Avatar : DatabaseEntity
{
	[Required]
	public string Source { get; set; }

	public List<User> Users { get; set; }
}