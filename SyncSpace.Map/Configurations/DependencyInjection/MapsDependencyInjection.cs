using Microsoft.Extensions.DependencyInjection;
using SyncSpace.Map.Factories.Markers;
using SyncSpace.Map.Factories.Polygons;
using SyncSpace.Map.JsInterops.Events;
using SyncSpace.Map.JsInterops.IconFactories;
using SyncSpace.Map.JsInterops.Maps;

namespace SyncSpace.Map.Configurations.DependencyInjection;

public static class MapsDependencyInjection
{
	public static IServiceCollection AddBlazorMaps(this IServiceCollection services)
	{
		AddJsInterops(services);
		AddFactories(services);
		return services;
	}

	private static void AddFactories(IServiceCollection services)
	{
		services.AddTransient<IMarkerFactory, MarkerFactory>();
		services.AddTransient<IUserMarkerFactory, UserMarkerFactory>();
		services.AddTransient<IVertexMarkerFactory, VertexMarkerFactory>();
		services.AddTransient<IIconFactory, IconFactory>();
		services.AddTransient<IPolylineFactory, PolylineFactory>();
		services.AddTransient<IPolygonFactory, PolygonFactory>();
		services.AddTransient<IEditablePolygonFactory, EditablePolygonFactory>();
		services.AddTransient<ICircleMarkerFactory, CircleMarkerFactory>();
		services.AddTransient<ICircleFactory, CircleFactory>();
	}

	private static void AddJsInterops(IServiceCollection services)
	{
		services.AddTransient<IEventedJsInterop, EventedJsInterop>();
		services.AddTransient<IIconFactoryJsInterop, IconFactoryJsInterop>();
		services.AddTransient<IMapJsInterop, MapJsInterop>();
	}
}