using Blazored.LocalStorage;
using GoalTracker.UI.Blazor.Dtos;
using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Models;
using GoalTracker.UI.Blazor.Providers;
using GoalTracker.UI.Blazor.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GoalTracker.UI.Blazor.Services;



public class AuthenticationService : BaseHttpService, IAuthenticationService
{

    private readonly AuthenticationStateProvider _authenticationStateProvider;
    public AuthenticationService(IClient client,
       Blazored.LocalStorage.ILocalStorageService  localStorage,
        AuthenticationStateProvider authenticationStateProvider
        ) : base(client,localStorage)
    {

        _authenticationStateProvider = authenticationStateProvider;
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
                await _localStorage.SetItemAsync("authToken", authenticationResponse.Token);
                //set claims in blazor and login stat
                await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedIn();
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Authentication error: {ex.Message}");
            // Or use a proper logging framework if available
            return false;
        }



    }

    public Task<string> Login(LoginRequestDto loginRequest)
    {
        throw new NotImplementedException();
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOut();
    }

    public Task<string> Register(RegisterRequestDto registerRequest)
    {
        //todo
        throw new NotImplementedException();
    }

 

   

   
}

