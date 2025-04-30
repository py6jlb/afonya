using Microsoft.AspNetCore.Components;
using Shared.Contracts;
using WebUI.Services.Interfaces;

namespace WebUI.Services;

public class AuthenticationService : IAuthenticationService
{
    private IHttpService _httpService;
    private NavigationManager _navigationManager;
    private ILocalStorageService _localStorageService;

    public AuthenticateResponse? AuthenticateResponse { get; private set; }


    public AuthenticationService(
        IHttpService httpService,
        NavigationManager navigationManager,
        ILocalStorageService localStorageService
    )
    {
        _httpService = httpService;
        _navigationManager = navigationManager;
        _localStorageService = localStorageService;
    }

    public async Task Initialize()
    {
        AuthenticateResponse = await _localStorageService.GetItem<AuthenticateResponse>("authenticateResponse");
    }

    public async Task Login(string username, string password)
    {
        AuthenticateResponse = await _httpService.Post<AuthenticateResponse>("/user/authenticate",
            new { Login = username, Password = password });
        await _localStorageService.SetItem("authenticateResponse", AuthenticateResponse);
    }

    public async Task Logout()
    {
        AuthenticateResponse = null;
        await _localStorageService.RemoveItem("authenticateResponse");
        _navigationManager.NavigateTo("login");
    }
}
