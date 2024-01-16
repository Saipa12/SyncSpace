using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using SyncSpace.Client.Services;
using SyncSpace.Map.Configurations.DependencyInjection;
using SyncSpace.Map.Factories.Polygons;

namespace SyncSpace.Client;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		builder.Services.AddMauiBlazorWebView();
		builder.Services.AddBlazorMaps();
		builder.Services.AddMudServices();
		builder.Services.AddScoped<IUserInformation, UserInformation>();
		builder.Services.AddScoped<AppPreferences>();
		builder.Services.AddScoped<IMapLocation, MapLocation>();
		builder.Services.AddScoped<IEditablePolygonFactory, EditablePolygonFactory>();
		builder.Services.AddScoped<AppPreferences>();
		return builder.Build();
	}
}