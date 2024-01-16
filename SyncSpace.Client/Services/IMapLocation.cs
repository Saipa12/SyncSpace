
/* Unmerged change from project 'SyncSpace.Client (net7.0-maccatalyst)'
Before:
using SyncSpace.Database.Entities;
using System;
After:
using System;
*/

/* Unmerged change from project 'SyncSpace.Client (net7.0-windows10.0.19041.0)'
Before:
using SyncSpace.Database.Entities;
using System;
After:
using System;
*/

/* Unmerged change from project 'SyncSpace.Client (net7.0-android33.0)'
Before:
using SyncSpace.Database.Entities;
using System;
After:
using System;
*/

/* Unmerged change from project 'SyncSpace.Client (net7.0-maccatalyst)'
Before:
using System.Threading.Tasks;
After:
using System.Threading.Tasks;
using SyncSpace.Database.Entities;
*/

/* Unmerged change from project 'SyncSpace.Client (net7.0-windows10.0.19041.0)'
Before:
using System.Threading.Tasks;
After:
using System.Threading.Tasks;
using SyncSpace.Database.Entities;
*/

/* Unmerged change from project 'SyncSpace.Client (net7.0-android33.0)'
Before:
using System.Threading.Tasks;
After:
using System.Threading.Tasks;
using SyncSpace.Database.Entities;
*/
namespace SyncSpace.Client.Services
{
	public interface IMapLocation
	{
		bool IsUse { get; set; }
		double Latitude { get; set; }
		double Longitude { get; set; }
	}
}