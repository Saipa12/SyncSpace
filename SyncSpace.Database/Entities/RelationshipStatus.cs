using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SyncSpace.Database.Enums;

namespace SyncSpace.Database.Entities;

public class RelationshipStatus : DatabaseEntity
{
	[Required, Column(TypeName = "character varying")]
	public RelationType Name { get; set; }
}