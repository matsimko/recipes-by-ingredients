using Microsoft.AspNetCore.Components;
using RbiFrontend.ApiAccess;
using RbiShared.DTOs;

namespace RbiFrontend.Services;

public class UserService
{
	private readonly NavigationManager _navigationManager;

	//private readonly Auth _auth;

	public UserDto? User { get; } = new() { Id = 1 };

    public UserService(NavigationManager navigationManager)
    {
		_navigationManager = navigationManager;
	}

    public bool IsAuthenticated()
	{
		return User != null;
	}

	public bool IsAuthenticated(long userId)
	{
		return User?.Id == userId;
	}

	public bool IsAuthenticated(UserDto? user)
	{
		return User != null && User.Id == user?.Id;
	}

	public void RequireAuthentication()
	{
		if(User == null)
		{
			_navigationManager.NavigateTo($"/login?RedirectUrl={_navigationManager.Uri}");
		}
	}


}
