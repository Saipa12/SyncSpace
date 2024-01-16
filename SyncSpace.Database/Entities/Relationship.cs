using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyncSpace.Database.Entities;

public class Relationship : DatabaseEntity
{
	[Required, NotMapped]
	public User Sender { get; set; }

	[Required, NotMapped]
	public User Receiver { get; set; }

	[Required]
	public RelationshipStatus RelationshipStatus { get; set; }
}