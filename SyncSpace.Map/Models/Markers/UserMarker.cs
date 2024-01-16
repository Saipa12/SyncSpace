using System.Timers;
using Microsoft.JSInterop;
using SyncSpace.Core.Controls;
using SyncSpace.Database.Entities;
using SyncSpace.Map;
using SyncSpace.Map.JsInterops.Events;

namespace SyncSpace.Client.Models.Markers;

public class UserMarker : Marker
{
	private Battery _Battery;

	private OnlineStatus _OnlineStatus;
	private static bool _IsOnline = true;

	public User User { get; private set; }

	public UserMarker(IJSObjectReference jsReference, IEventedJsInterop eventedJsInterop, User user) : base(jsReference, eventedJsInterop)
	{
		User = user;
	}

	public async Task SetUser(User user)
	{
		User = user;
		await Update();
	}

	public async Task FillAsync()
	{
		_Battery = new Battery(await JsReference.InvokeAsync<IJSObjectReference>("createBattery"));
		_OnlineStatus = new OnlineStatus(await JsReference.InvokeAsync<IJSObjectReference>("createStatus"));
		await _Battery.InitializeDeprecated();
		await _OnlineStatus.InitializeDeprecated();
		System.Timers.Timer timer = new System.Timers.Timer(300);
		timer.Elapsed += new ElapsedEventHandler(RefreshBattery);
		//timer.Enabled = true;
		await Update();
	}

	public async Task Update()
	{
		await _Battery.SetBatteryLevel(User.ChargeLevel);
		_IsOnline = !_IsOnline;
		await _OnlineStatus.SetStatus(_IsOnline);
	}

	private async void RefreshBattery(object sender, ElapsedEventArgs e)
	{
		//await _Battery.SetBatteryLevel(new Random().Next(0, 100));
	}
}