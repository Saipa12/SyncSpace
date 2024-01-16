using System.Net.NetworkInformation;
using SyncSpace.Core.Exceptions;

namespace SyncSpace.Core.Helpers;

public static class InternetHelper
{
	public static async Task<bool> TestInternetConnection(bool throwOnError = false)
	{
		try
		{
			using var ping = new Ping();
			var reply = await ping.SendPingAsync("google.com");
			return reply != null && reply.Status == IPStatus.Success;
		}
		catch
		{
			if (throwOnError)
				throw new InternetConnectionException();
			return false;
		}
	}
}