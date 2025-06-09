using Blazored.LocalStorage;
using System;
using System.Text.Json;
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

        protected Response<T> ConvertApiExceptions<T>(ApiException exception)
        {
            // Handle 201 Created as success (for your current issue)
            if (exception.StatusCode == 201)
            {
                try
                {
                    var data = JsonSerializer.Deserialize<T>(exception.Response, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    return new Response<T>
                    {
                        Success = true,
                        Data = data,
                        Message = "Created successfully"
                    };
                }
                catch (JsonException)
                {
                    // If deserialization fails, still return success but without data
                    return new Response<T>
                    {
                        Success = true,
                        Message = "Created successfully"
                    };
                }
            }

            // Handle other status codes
            if (exception.StatusCode == 400)
            {
                return new Response<T>
                {
                    Message = "Invalid data was submitted",
                    ValidationErrors = exception.Response,
                    Success = false
                };
            }
            else if (exception.StatusCode == 404)
            {
                return new Response<T>
                {
                    Message = "The record was not found",
                    ValidationErrors = exception.Response,
                    Success = false
                };
            }
            else if (exception.StatusCode == 401)
            {
                return new Response<T>
                {
                    Message = "Unauthorized access",
                    Success = false
                };
            }
            else if (exception.StatusCode == 403)
            {
                return new Response<T>
                {
                    Message = "Access forbidden",
                    Success = false
                };
            }
            else
            {
                return new Response<T>()
                {
                    Message = "Something went wrong, please try again later",
                    Success = false
                };
            }
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
