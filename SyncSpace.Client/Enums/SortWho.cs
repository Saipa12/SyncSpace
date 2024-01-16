using System.ComponentModel.DataAnnotations;

namespace SyncSpace.Client.Enums;

public enum SortWho
{
	[Display(Name = "Friends")]
	Friends,

	[Display(Name = "Requests")]
	Requests,

	[Display(Name = "Send Requests")]
	Send_requests,

	[Display(Name = "Rejected")]
	Rejected,

	[Display(Name = "By Group")]
	Group
}