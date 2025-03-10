using Blazored.LocalStorage;
using GoalTracker.UI.Blazor.Dtos;
using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Models;
using GoalTracker.UI.Blazor.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GoalTracker.UI.Blazor.Services;



public class AuthenticationService : BaseHttpService, IAuthenticationService
{
    public AuthenticationService(IClient client, Blazored.LocalStorage.ILocalStorageService localStorage) : base(client, localStorage)
    {
    }

    //private readonly HttpClient _httpClient;

    //private readonly Interfaces.Services.ILocalStorageService _localStorage;


    public async Task<bool> AuthenticateAsync(string email, string password)
    {
        try {

            LoginCommand loginCommand = new LoginCommand() { Email = email, Password = password };

            var authenticationResponse = await _client.LoginAsync(loginCommand);
            if (!string.IsNullOrEmpty(authenticationResponse.Token))
            {
                await _localStorage.SetItemAsync("token", authenticationResponse.Token);
                //set claims in blazor and login stat
                return true;
            }
            return false;
        }
        catch(Exception) {

            return false;
        }   
        
        

    }

    public Task<string> Login(LoginRequestDto loginRequest)
    {
        throw new NotImplementedException();
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("token");
        //remove claims
    }

    public Task<string> Register(RegisterRequestDto registerRequest)
    {
        //todo
        throw new NotImplementedException();
    }
}

