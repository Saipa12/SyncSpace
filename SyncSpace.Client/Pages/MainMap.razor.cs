using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using SyncSpace.Client.Models.Markers;
using SyncSpace.Database.Contexts;
using SyncSpace.Database.Entities;
using SyncSpace.Map;
using SyncSpace.Map.Factories.Markers;
using SyncSpace.Map.Factories.Polygons;
using SyncSpace.Map.Models.Polygons;
using C = SyncSpace.Core.Controls;

using Db = SyncSpace.Database.Entities;

using Location = SyncSpace.Database.Entities.Location;

using M = SyncSpace.Map;

using MB = MudBlazor;
using T = System.Timers;

namespace SyncSpace.Client.Pages
{
    public partial class MainMap
    {
        private bool IsAreaEditorVisible = false;
        private bool darkModed = false;
        private CancellationTokenSource _CancelTokenSource;
        private string _MarkerUserName;
        private string _UserAvatar;
        private int _UserBatteryLvl;
        private string _BatteryColor;
        private double x = 0.01;

        //private string connString = "Host=localhost;Port=5432;Database=SyncSpace;Username=postgres;Password=root";
        private List<User> _FriendList;

        private M.Map _MapRef;
        private Microsoft.Maui.Devices.Sensors.Location _HomeLocation;
        private C.BottomSheetView _BottomSheet;
        private MB.MudCarousel<object> _Carousel;
        private C.Battery _Battery;
        private EditablePolygon _EditablePolygon;
        private UserMarker _AuthorizedUserMarker;
        private T.Timer timer;
        private List<Area> _Areas = new();
        public List<UserMarker> FriendMarkersList;

        private readonly MapOptions _MapOptions = new()
        {
            DivId = "map-id",
            Zoom = 13,
            Center = new LatLng(5, 3),
            UrlTileLayer = "https://{s}.tile.openstreetmap.fr/hot/{z}/{x}/{y}.png",
            SubOptions = new MapSubOptions()
            {
                Attribution = "&copy; <a lhref='http://www.openstreetmap.org/copyright'>OpenStreetMap</a>",
                TileSize = 512,
                ZoomOffset = -1,
                MaxZoom = 19,
            }
        };

        private readonly MarkerOptions _VertexMarkerOptions = new()
        {
            Draggable = true,
            AutoPan = true,
            AutoPanSpeed = 10,
            AutoPanPadding = new M.Point(20, 20),
            RiseOnHover = true
        };

        [Inject]
        private IUserMarkerFactory UserMarkerFactory { get; init; }

        [Inject]
        private IEditablePolygonFactory EditablePolygonFactory { get; init; }

        [Inject]
        private IIconFactory IconFactory { get; init; }

        protected override void OnInitialized()
        {
            FriendMarkersList = new List<UserMarker>();
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            await MainThread.InvokeOnMainThreadAsync(RefreshLocation);
            MainThread.BeginInvokeOnMainThread(() => _MapOptions.Center = new LatLng(_HomeLocation.Latitude, _HomeLocation.Longitude));
            if (MapLocation.IsUse)
            {
                MapLocation.IsUse = false;
                await _MapRef.SetView(new LatLng(MapLocation.Latitude, MapLocation.Longitude));
            }
            else
            {
                await _MapRef.SetView(new LatLng(_HomeLocation.Latitude, _HomeLocation.Longitude));
            }
            await RefreshLocation();

            var markerOptions = new MarkerOptions()
            {
                IconRef = await IconFactory.Create(new IconOptions()
                {
                    IconUrl = AuthorizedUser.User.Avatar.Source,
                    IconSize = new M.Point(38, 95)
                }),
                Draggable = false,
                AutoPan = true,
                AutoPanSpeed = 10,
                AutoPanPadding = new M.Point(20, 20),
                RiseOnHover = true
            };

            _AuthorizedUserMarker = await UserMarkerFactory.CreateAndAddToMap(new LatLng(_HomeLocation.Latitude, _HomeLocation.Longitude), _MapRef, markerOptions, AuthorizedUser.User);
            await _AuthorizedUserMarker.OnClick(async (MouseEvent mouseEvent) => { await OnUserMarkerClick(_AuthorizedUserMarker); });
            await _AuthorizedUserMarker.OnDblClick(async (MouseEvent mouseEvent) => { await _BottomSheet.CloseExpander(); });

            timer = new T.Timer(5000);
            timer.Elapsed += new T.ElapsedEventHandler(RefreshData);
            timer.Enabled = true;

            await _BottomSheet.Initialize();
        }

        private async void RefreshData(object sender, T.ElapsedEventArgs e) => await RefreshLocation();

        private long _LastSelectedArea = 0;

        private async Task ChangeAreaEditorVisibility()
        {
            IsAreaEditorVisible = !IsAreaEditorVisible;

            if (IsAreaEditorVisible)
            {
                using DatabaseContext model = new(AuthorizedUser.ConnectionString);
                _Areas = await model.Areas.Include(x => x.TrackedUser).ToListAsync();
                await ChangeEditablePolygon((int)_LastSelectedArea);
                await _MapRef.FlyToBounds(await _EditablePolygon.GetBounds(), 400);
            }
            else
            {
                if (_EditablePolygon != null)
                {
                    await _EditablePolygon.Remove();
                    _EditablePolygon = null;
                    _LastSelectedArea = _SelectedAreaIndex;
                }
            }
        }

        public async Task GetAreas()
        {
            using DatabaseContext model = new(AuthorizedUser.ConnectionString);
            _Areas = await model.Areas.Include(x => x.TrackedUser).ToListAsync();
            await ChangeEditablePolygon(_SelectedAreaIndex);
        }

        public async Task DrawArea()
        {
            if (_EditablePolygon != null)
                await _EditablePolygon.Remove();
        }

        public static bool IsPointInPolygon(List<PointF> polygon, PointF point)
        {
            int intersections = 0;

            for (int i = 0; i < polygon.Count; i++)
            {
                var p1 = polygon[i];
                var p2 = polygon[(i + 1) % polygon.Count];

                if (p1.Y == p2.Y) continue;

                if ((point.Y >= p1.Y && point.Y < p2.Y) || (point.Y >= p2.Y && point.Y < p1.Y))
                {
                    double x = (point.Y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y) + p1.X;
                    if (x < point.X)
                    {
                        intersections++;
                    }
                }
            }

            return intersections % 2 != 0;
        }

        private async Task ChangeEditablePolygon(int index)
        {
            if (_EditablePolygon != null)
                await _EditablePolygon.Remove();

            var selectedArea = _Areas[index];

            var points = new List<LatLng>();

            for (var i = 0; i < selectedArea.Longitudes.Length; i++)
                points.Add(new LatLng(selectedArea.Latitudes[i], selectedArea.Longitudes[i]));

            _EditablePolygon = await EditablePolygonFactory.CreateAndAddToMap(points, _MapRef, markerOptions: _VertexMarkerOptions);
        }

        private bool IsEditing = false;

        private async Task DefaultMode()
        {
            await _EditablePolygon.EnableDefaultMode();
            IsEditing = false;
        }

        private async Task AddVertex()
        {
            await _EditablePolygon.EnableAddMode();
            IsEditing = true;
        }

        private async Task RemoveVertex()
        {
            await _EditablePolygon.EnableDeleteMode();
            IsEditing = true;
        }

        private async Task SaveZone()
        {
            _Areas[_SelectedAreaIndex].Latitudes = (await _EditablePolygon.GetLatLngs()).Select(x => x.Lat).ToArray();
            _Areas[_SelectedAreaIndex].Longitudes = (await _EditablePolygon.GetLatLngs()).Select(x => x.Lng).ToArray();
            using (DatabaseContext model = new(AuthorizedUser.ConnectionString))
            {
                model.Areas.Update(_Areas[_SelectedAreaIndex]);
                await model.SaveChangesAsync();
            }
        }

        private int _SelectedAreaIndex = 0;

        private async Task OnCarouselItemChanged(int index)
        {
            _SelectedAreaIndex = index;
            await ChangeEditablePolygon(index);
            await _MapRef.FlyToBounds(await _EditablePolygon.GetBounds(), 400);
        }

        private async Task OnUserMarkerClick(UserMarker userMarker)
        {
            await _BottomSheet.OpenExpander(60);
            _UserBatteryLvl = userMarker.User.ChargeLevel;
            _MarkerUserName = userMarker.User.Name;
            _UserAvatar = userMarker.User.Avatar.Source;
            await InvokeAsync(StateHasChanged);
        }

        public async Task RefreshLocation()
        {
            _HomeLocation = await GetCurrentLocation();
            var Location = new Location();
            Location.Latitude = _HomeLocation.Latitude;
            Location.Longitude = _HomeLocation.Longitude;
            await SendLocationToDb(Location);
            await ShowWhereMyFriends();

            LatLng mapCenter = await _MapRef.GetCenter();
            int zoom = (int)await _MapRef.GetZoom();

            await JSRuntime.InvokeVoidAsync("getAddress", mapCenter.Lat, mapCenter.Lng, zoom);
        }

        private async Task SendLocationToDb(Location location)
        {
            using (DatabaseContext model = new(AuthorizedUser.ConnectionString))
            {
                var lastLocation = await model.Locations.OrderBy(x => x.Id).LastOrDefaultAsync(x => x.User.Id == AuthorizedUser.User.Id);

                if (Distance(lastLocation.Latitude, lastLocation.Longitude, location.Latitude, location.Longitude) > 0.01)
                {
                    var dbLocation = new Db.Location()
                    {
                        UserId = AuthorizedUser.User.Id,
                        Latitude = location.Latitude,
                        Longitude = location.Longitude
                    };

                    await model.Locations.AddAsync(dbLocation);
                    if (_AuthorizedUserMarker is not null)
                        await _AuthorizedUserMarker.SetLatLng(new LatLng(lastLocation.Latitude, lastLocation.Longitude));

                    await InvokeAsync(StateHasChanged);
                    model.SaveChanges();
                }
            };
        }

        private async Task ShowWhereMyFriends()
        {
            using (var model = new DatabaseContext(AuthorizedUser.ConnectionString))
            {
                _FriendList = await model.Users.Where(x => x.Id != AuthorizedUser.User.Id).ToListAsync();
                var Avatars = await model.Avatars.ToListAsync();
                foreach (var friend in _FriendList)
                {
                    var friendsLocation = await model.Locations.FirstOrDefaultAsync(x => x.User == friend);
                    if (friendsLocation is not null)
                    {
                        var markerOptions = new MarkerOptions()
                        {
                            IconRef = await IconFactory.Create(new IconOptions()
                            {
                                IconUrl = friend.Avatar.Source,
                                IconSize = new M.Point(38, 95),
                            }),
                            Draggable = false,
                            AutoPan = true,
                            AutoPanSpeed = 10,
                            AutoPanPadding = new M.Point(20, 20),
                            RiseOnHover = true
                        };
                        if (FriendMarkersList.Any(x => x.User.Id == friend.Id))
                        {
                            await FriendMarkersList.First(x => x.User.Id == friend.Id).SetLatLng(new LatLng(friendsLocation.Latitude, friendsLocation.Longitude));
                        }
                        else
                        {
                            var markerWithOptions = await UserMarkerFactory.CreateAndAddToMap(new LatLng(friendsLocation.Latitude, friendsLocation.Longitude), _MapRef, markerOptions, friend);
                            await markerWithOptions.OnClick(async (MouseEvent mouseEvent) => { await OnUserMarkerClick(markerWithOptions); });
                            await markerWithOptions.OnDblClick(async (MouseEvent mouseEvent) => { await _BottomSheet.CloseExpander(); });
                            FriendMarkersList.Add(markerWithOptions);
                        }
                    }
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        public void OpenChangeUserInfo() => Navigation.NavigateTo("ChangeUserInfo");

        public void OpenFriends() => Navigation.NavigateTo("FriendsWindow");

        public void OpenSettings() => Navigation.NavigateTo("SettingsWindow");

        public void OpenChats() => Navigation.NavigateTo("Chats");

        private async Task<Microsoft.Maui.Devices.Sensors.Location> GetCurrentLocation()
        {
            try
            {
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(15));
                _CancelTokenSource = new CancellationTokenSource();
                return await MainThread.InvokeOnMainThreadAsync(async () => await Geolocation.Default.GetLocationAsync(request, _CancelTokenSource.Token));
            }
            finally
            {
            }
        }

        public static double Distance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Радиус Земли в километрах
            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLon = (lon2 - lon1) * Math.PI / 180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c;
            return distance;
        }

        private async Task ZoomIn() => await _MapRef.ZoomIn(1);

        private async Task ZoomOut() => await _MapRef.ZoomOut(1);

        private async Task FindMe() => await _MapRef.FlyTo(new LatLng(_HomeLocation.Latitude, _HomeLocation.Longitude), 17);
    }
}