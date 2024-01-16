using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyncSpace.Database.Entities
{
	public class Area : DatabaseEntity
	{
		[Required]
		public string AreaName { get; set; }

		[Required]
		public double[] Latitudes { get; set; }

		[Required]
		public double[] Longitudes { get; set; }

		[Required]
		public bool IsInside { get; set; }

		[Required, NotMapped]
		public User User { get; set; }

		[Required]
		public User TrackedUser { get; set; }
	}
}