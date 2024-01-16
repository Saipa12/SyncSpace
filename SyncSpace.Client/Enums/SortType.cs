using System.ComponentModel.DataAnnotations;

namespace SyncSpace.Client.Enums;

public enum SortType
{
	[Display(Name = "Name")]
	Name,

	[Display(Name = "BatteryLvl")]
	BatteryLvl,

	[Display(Name = "Distance")]
	Distance,

	[Display(Name = "Online")]
	Online
}