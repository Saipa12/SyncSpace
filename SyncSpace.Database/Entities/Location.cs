using System.ComponentModel.DataAnnotations;

namespace SyncSpace.Database.Entities;

public class Location : DatabaseEntity
{
    public long UserId { get; set; }

    [Required]
    public User User { get; set; }

    [Required]
    public double Latitude { get; set; }

    [Required]
    public double Longitude { get; set; }
}