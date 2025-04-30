using System;
using Shared.Contracts;

namespace WebUI.Services.Interfaces;

public interface IAuthenticationService
{
        AuthenticateResponse AuthenticateResponse { get; }
        Task Initialize();
        Task Login(string username, string password);
        Task Logout();
}
