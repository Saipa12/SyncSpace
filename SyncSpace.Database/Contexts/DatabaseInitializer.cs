using SyncSpace.Database.Entities;
using SyncSpace.Database.Enums;

namespace SyncSpace.Database.Contexts;

internal class DatabaseInitializer
{
	public async Task InitializeDatabaseAsync(DatabaseContext context)
	{
		var userAvatar = new Avatar() { Source = "https://kartinkin.net/uploads/posts/2021-02/1612367107_16-p-anime-tyanka-litso-art-kartinki-17.jpg" };
		var tractedUserAvatar = new Avatar() { Source = "https://mobimg.b-cdn.net/v3/fetch/bf/bf34d106e9d8edb721b7d4ec9ad6bda7.jpeg" };
		var adamUserAvatar = new Avatar() { Source = "https://avatars.mds.yandex.net/i?id=8ae98159356021803c387ff99ea6a63ba0c9285f-9106994-images-thumbs&n=13https://avatars.mds.yandex.net/i?id=8ae98159356021803c387ff99ea6a63ba0c9285f-9106994-images-thumbs&n=13" };
		var kostyaUserAvatar = new Avatar() { Source = "https://papik.pro/uploads/posts/2021-12/1640469744_52-papik-pro-p-krasivie-risunki-tyanok-57.jpg" };
		var sanyaUserAvatar = new Avatar() { Source = "https://kartinkin.net/uploads/posts/2021-02/1612367193_30-p-anime-tyanka-litso-art-kartinki-32.jpg" };
		var timurUserAvatar = new Avatar() { Source = "https://i4.imageban.ru/out/2015/12/01/83279fda2cfd36fcf07d1ba5d64c2170.jpg" };
		await context.AddRangeAsync(userAvatar, tractedUserAvatar, adamUserAvatar, kostyaUserAvatar, sanyaUserAvatar, timurUserAvatar);

		var user = new User() { Name = "Spuipa", Email = "spipa@gmail.com", MacAddress = "asfafs", ChargeLevel = 10, BatteryState = false, Avatar = tractedUserAvatar };
		var tractedUser = new User() { Name = "Saipa", Email = "ssaassaass@gmail.com", MacAddress = "678678", ChargeLevel = 89, BatteryState = true, Avatar = userAvatar };
		var adamUser = new User() { Name = "Adam", Email = "adam600@gmail.com", MacAddress = "675678", ChargeLevel = 98, BatteryState = false, Avatar = adamUserAvatar };
		var kostyaUser = new User() { Name = "Kartoshka", Email = "belarus@gmail.com", MacAddress = "677978", ChargeLevel = 3, BatteryState = true, Avatar = kostyaUserAvatar };
		var sanyaUser = new User() { Name = "Oduvanchick", Email = "Plya@gmail.com", MacAddress = "603178", ChargeLevel = 56, BatteryState = true, Avatar = sanyaUserAvatar };
		var timurUser = new User() { Name = "Timur", Email = "dolgnick@gmail.com", MacAddress = "6722678", ChargeLevel = 7, BatteryState = false, Avatar = timurUserAvatar };
		await context.AddRangeAsync(user, tractedUser, adamUser, kostyaUser, sanyaUser, timurUser);

		await context.Areas.AddRangeAsync(
			new Area() { IsInside = true, AreaName = "Zone 1", Latitudes = new double[] { 23, 27, 29 }, Longitudes = new double[] { 28, 23, 28 }, User = user, TrackedUser = tractedUser },
			new Area() { IsInside = false, AreaName = "Zone 51", Latitudes = new double[] { 20, 19, 18, 16 }, Longitudes = new double[] { 28, 23, 21, 20 }, User = user, TrackedUser = tractedUser });

		await context.Locations.AddRangeAsync(
			new Location() { Latitude = 22, Longitude = 54, User = user },
			new Location() { Latitude = 13, Longitude = 73, User = tractedUser },
			new Location() { Latitude = 78, Longitude = 12, User = adamUser },
			new Location() { Latitude = 44, Longitude = 49, User = kostyaUser },
			new Location() { Latitude = 50, Longitude = 60, User = sanyaUser },
			new Location() { Latitude = 51, Longitude = 61, User = timurUser });

		var sent = new RelationshipStatus() { Name = RelationType.Sent };
		var accepted = new RelationshipStatus() { Name = RelationType.Accepted };
		var rejected = new RelationshipStatus() { Name = RelationType.Rejected };
		await context.AddRangeAsync(sent, accepted, rejected);

		await context.Relationships.AddRangeAsync(
			new Relationship() { Sender = user, Receiver = tractedUser, RelationshipStatus = sent },
			new Relationship() { Sender = adamUser, Receiver = user, RelationshipStatus = sent },
			new Relationship() { Sender = kostyaUser, Receiver = user, RelationshipStatus = accepted },
			new Relationship() { Sender = timurUser, Receiver = user, RelationshipStatus = rejected },
			new Relationship() { Sender = user, Receiver = sanyaUser, RelationshipStatus = sent });

		await context.SaveChangesAsync();
	}
}