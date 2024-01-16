using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SyncSpace.Client.Enums;
using SyncSpace.Database.Contexts;
using SyncSpace.Database.Entities;
using SyncSpace.Database.Enums;

namespace SyncSpace.Client.Pages;

public partial class FriendsWindow
{
	private List<User> Users;
	private List<User> SearchResult;
	private List<Relationship> Relationships;
	private List<Relationship> SearchRelationships;
	private SortWho _SortWhoCategory;
	private SortType _SortTypeCategory;
	private bool _isHidennCount = false;
	private string _sizeText = "20px";
	private bool _SortDescendStyle { get; set; } = true;

	protected override void OnInitialized()

	{
		MapLocation.IsUse = false;
		_SortTypeCategory = SortType.Name;
		_SortWhoCategory = SortWho.Friends;
		RefreshRelationStatus();
	}

	public void RefreshRelationStatus()
	{
		DatabaseContext model = new(AuthorizedUser.ConnectionString);
		Relationships = model.Relationships.Include(x => x.RelationshipStatus).Include(x => x.Sender).Include(x => x.Receiver).Where(x => x.Sender.Id == AuthorizedUser.User.Id || x.Receiver.Id == AuthorizedUser.User.Id).ToList();
		//Получили все взаимоотношения с которыми связан наш пользователь
		SearchRelationships = Relationships.Where(x => x.RelationshipStatus.Name == RelationType.Accepted).ToList();
		//выделили тех кто в друзьях
		Users = model.Users.Include(x => x.Avatar).Include(x => x.ReciveRelationships).Include(x => x.SendRelationships).Include(x => x.Locations).Where(x => (x.SendRelationships.Any(x => Relationships.Contains(x)) || x.ReciveRelationships.Any(x => Relationships.Contains(x))) && x.Id != AuthorizedUser.User.Id).ToList();
		//получили всех пользователей у которых есть взаимоотношение с нашим пользователем
		SearchResult = Users.Where(x => x.SendRelationships.Any(x => SearchRelationships.Contains(x)) || x.ReciveRelationships.Any(x => SearchRelationships.Contains(x))).ToList();
		//Выделили его друзей
	}

	private void OnValueChanged(string value)
	{
		// var regex = new Regex(value, RegexOptions.IgnoreCase);
		//SearchResult = Users.Where(x => regex.IsMatch(x.Name)).OrderBy(x => x.Name.IndexOf(regex.Match(x.Name).Value)).ToList();
		RefreshRelationStatus();
		HandleOptionChanged(_SortWhoCategory);
		SearchResult = SearchResult.Where(x => x.Name.ToLower().StartsWith(value.ToLower())).ToList();
	}

	private void HandleOptionChanged(SortType value)
	{
		_SortTypeCategory = value;
		switch (value)
		{
			case SortType.Name:
				SearchResult = _SortDescendStyle ? SearchResult.OrderBy(x => x.Name).ToList() : SearchResult.OrderByDescending(x => x.Name).ToList();
				break;

			case SortType.Online:
				SearchResult = _SortDescendStyle ? SearchResult.OrderBy(x => x.Name).ToList() : SearchResult.OrderByDescending(x => x.Name).ToList();
				break;

			case SortType.Distance:
				SearchResult = _SortDescendStyle ? SearchResult.OrderBy(x => x.Name).ToList() : SearchResult.OrderByDescending(x => x.Name).ToList();
				break;

			case SortType.BatteryLvl:
				SearchResult = _SortDescendStyle ? SearchResult.OrderByDescending(x => x.ChargeLevel).ToList() : SearchResult.OrderBy(x => x.ChargeLevel).ToList();
				break;
		}
	}

	private void HandleOptionChanged(SortWho value)
	{
		_SortWhoCategory = value;
		switch (value)
		{
			case SortWho.Friends:
				SearchRelationships = Relationships.Where(x => x.RelationshipStatus.Name == RelationType.Accepted).ToList();
				SearchResult = Users.Where(x => x.SendRelationships.Any(x => SearchRelationships.Contains(x)) || x.ReciveRelationships.Any(x => SearchRelationships.Contains(x))).ToList();
				break;

			case SortWho.Send_requests:
				SearchRelationships = Relationships.Where(x => x.RelationshipStatus.Name == RelationType.Sent).ToList();
				SearchResult = Users.Where(x => x.ReciveRelationships.Any(x => SearchRelationships.Contains(x))).ToList();
				break;

			case SortWho.Requests:
				SearchRelationships = Relationships.Where(x => x.RelationshipStatus.Name == RelationType.Sent).ToList();
				SearchResult = Users.Where(x => x.SendRelationships.Any(x => SearchRelationships.Contains(x))).ToList();
				break;

				//case SortWho.Rejected:
				//    SearchResult = null;
				//    _CountUsersCategory = "Group" + SearchResult.Count.ToString(); ;
				//    break;
		}
	}

	public void OpenMap()
	{
		Navigation.NavigateTo("MainMap");
	}

	public void OnDescendingClick()
	{
		_SortDescendStyle = !_SortDescendStyle;
		HandleOptionChanged(_SortTypeCategory);
	}

	public void DeleteFriend(User user)
	{
		DatabaseContext model = new(AuthorizedUser.ConnectionString);
		var relationship = Relationships.First(x => (x.Receiver.Id == user.Id && x.Sender.Id == AuthorizedUser.User.Id) || (x.Receiver.Id == AuthorizedUser.User.Id && x.Sender.Id == user.Id));
		relationship.RelationshipStatus.Name = RelationType.Rejected;
		relationship.Sender = user;
		relationship.Receiver = AuthorizedUser.User;

		var dbRelationship = model.Relationships.Include(x => x.RelationshipStatus).Include(x => x.Sender).Include(x => x.Receiver).First(x => (x.Receiver.Id == user.Id && x.Sender.Id == AuthorizedUser.User.Id) || (x.Receiver.Id == AuthorizedUser.User.Id && x.Sender.Id == user.Id));
		dbRelationship.Sender = model.Users.First(x => x.Id == user.Id);
		dbRelationship.Receiver = model.Users.First(x => x.Id == AuthorizedUser.User.Id);
		dbRelationship.RelationshipStatus = model.RelationshipStatuses.First(x => x.Name == RelationType.Rejected);
		model.SaveChanges();
		RefreshRelationStatus();
	}

	public void MoveTo(User user)
	{
		MapLocation.Longitude = user.Locations.LastOrDefault().Longitude;
		MapLocation.Latitude = user.Locations.LastOrDefault().Latitude;
		MapLocation.IsUse = true;
		Navigation.NavigateTo("MainMap");
	}

	public void AcceptFriend(User user)
	{
		DatabaseContext model = new(AuthorizedUser.ConnectionString);
		var relationship = Relationships.First(x => (x.Receiver.Id == user.Id && x.Sender.Id == AuthorizedUser.User.Id) || (x.Receiver.Id == AuthorizedUser.User.Id && x.Sender.Id == user.Id));
		relationship.RelationshipStatus.Name = RelationType.Accepted;

		var dbRelationship = model.Relationships.Include(x => x.RelationshipStatus).Include(x => x.Sender).Include(x => x.Receiver).First(x => (x.Receiver.Id == user.Id && x.Sender.Id == AuthorizedUser.User.Id) || (x.Receiver.Id == AuthorizedUser.User.Id && x.Sender.Id == user.Id));
		dbRelationship.RelationshipStatus = model.RelationshipStatuses.First(x => x.Name == RelationType.Accepted);
		model.SaveChanges();
		RefreshRelationStatus();
	}

	public void RejectFriend(User user)
	{
		DatabaseContext model = new(AuthorizedUser.ConnectionString);
		var relationship = Relationships.First(x => (x.Receiver.Id == user.Id && x.Sender.Id == AuthorizedUser.User.Id) || (x.Receiver.Id == AuthorizedUser.User.Id && x.Sender.Id == user.Id));
		relationship.RelationshipStatus.Name = RelationType.Rejected;

		var dbRelationship = model.Relationships.Include(x => x.RelationshipStatus).Include(x => x.Sender).Include(x => x.Receiver).First(x => (x.Receiver.Id == user.Id && x.Sender.Id == AuthorizedUser.User.Id) || (x.Receiver.Id == AuthorizedUser.User.Id && x.Sender.Id == user.Id));
		dbRelationship.RelationshipStatus = model.RelationshipStatuses.First(x => x.Name == RelationType.Rejected);
		model.SaveChanges();
		RefreshRelationStatus();
	}

	public void CancelRequest(User user)
	{
		DatabaseContext model = new(AuthorizedUser.ConnectionString);
		var relationship = Relationships.First(x => (x.Receiver.Id == user.Id && x.Sender.Id == AuthorizedUser.User.Id) || (x.Receiver.Id == AuthorizedUser.User.Id && x.Sender.Id == user.Id));
		relationship.RelationshipStatus.Name = RelationType.Rejected;

		var dbRelationship = model.Relationships.Include(x => x.RelationshipStatus).Include(x => x.Sender).Include(x => x.Receiver).First(x => (x.Receiver.Id == user.Id && x.Sender.Id == AuthorizedUser.User.Id) || (x.Receiver.Id == AuthorizedUser.User.Id && x.Sender.Id == user.Id));
		model.Relationships.Remove(dbRelationship);
		model.SaveChanges();
		RefreshRelationStatus();
	}

	public string GetDescription(Enum enu, string enumName)
	{
		var enumType = Type.GetType("SyncSpace.Client.Enums." + enumName);
		var memberInfos = enumType.GetMember(enu.ToString());
		var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);
		var valueAttributes = enumValueMemberInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
		return ((DisplayAttribute)valueAttributes[0]).Name;
	}

	public string GetCountDescription(SortWho value)
	{
		switch (value)
		{
			case SortWho.Friends:
				SearchRelationships = Relationships.Where(x => x.RelationshipStatus.Name == RelationType.Accepted).ToList();
				return Users.Where(x => x.SendRelationships.Any(x => SearchRelationships.Contains(x)) || x.ReciveRelationships.Any(x => SearchRelationships.Contains(x))).ToList().Count.ToString();

			case SortWho.Send_requests:
				SearchRelationships = Relationships.Where(x => x.RelationshipStatus.Name == RelationType.Sent).ToList();
				return Users.Where(x => x.ReciveRelationships.Any(x => SearchRelationships.Contains(x))).ToList().Count.ToString();

			case SortWho.Requests:
				SearchRelationships = Relationships.Where(x => x.RelationshipStatus.Name == RelationType.Sent).ToList();
				return Users.Where(x => x.SendRelationships.Any(x => SearchRelationships.Contains(x))).ToList().Count.ToString();

			default: return "0";
		}
	}

	public void OnOpen()
	{
		_isHidennCount = true;
	}

	public void OnClose()
	{
		_isHidennCount = false;
	}
}