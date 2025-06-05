using Blazored.LocalStorage;
using System;
using System.Threading.Tasks;

namespace GoalTracker.UI.Blazor.Services.Base
{
    public class BaseHttpService
    {
        protected IClient _client;
        protected readonly ILocalStorageService _localStorage;
        public BaseHttpService(IClient client,ILocalStorageService localStorage)
        {
            _client = client;
            _localStorage = localStorage;
        }

        protected Response<Guid> ConvertApiExceptions<Guid>(ApiException exception)
        {

            if (exception.StatusCode == 400)
            {
                return new Response<Guid> 
                { Message = "Invalid data was submitted",
                    ValidationErrors = exception.Response, Success = false };
            }
            else if (exception.StatusCode == 404)
            {
                return new Response<Guid>
                {
                    Message = "The record was not found",
                    ValidationErrors = exception.Response,
                    Success = false
                };
            }
            else { return new Response<Guid>() { Message="Somthing went wrong, please try again later",Success = false}; }
        }

        protected async Task AddBearerToken()
        {
            // Change "token" to match what you're using elsewhere (likely "authToken")
            if (await _localStorage.ContainKeyAsync("authToken"))
            {
                // Clear any existing authorization header
                if (_client.HttpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _client.HttpClient.DefaultRequestHeaders.Remove("Authorization");
                }

                // Add the token
                var token = await _localStorage.GetItemAsync<string>("authToken");
                _client.HttpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                Console.WriteLine("Bearer token added from BaseHttpService");
            }
            else
            {
                Console.WriteLine("No token found in storage - request will be unauthorized");
            }
        }

    }
}
