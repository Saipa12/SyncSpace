using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyncSpace.Database.Entities;

public class User : DatabaseEntity
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string MacAddress { get; set; }

    [Required, MinLength(0), MaxLength(100)]
    public int ChargeLevel { get; set; }

    [Required]
    public bool BatteryState { get; set; }

    public List<Location> Locations { get; set; }

    public Avatar Avatar { get; set; }
    public DateTime? BirthDay { get; set; }

	public List<Area> Areas { get; set; }

	[NotMapped]
	public List<Relationship> SendRelationships { get; set; }

	[NotMapped]
	public List<Relationship> ReciveRelationships { get; set; }
}