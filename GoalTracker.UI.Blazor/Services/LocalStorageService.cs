using GoalTracker.UI.Blazor.Interfaces.Services;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Threading.Tasks;

namespace GoalTracker.UI.Blazor.Services
{


    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<T> GetItemAsync<T>(string key)
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

            if (json == null)
                return default;

            return JsonSerializer.Deserialize<T>(json);
        }
        public async Task SetItemAsync<T>(string key, T value)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
        }

        public async Task RemoveItemAsync(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }

        Task<T> ILocalStorageService.GetItemAsync<T>(string key)
        {
            throw new System.NotImplementedException();
        }

        Task ILocalStorageService.SetItemAsync<T>(string key, T value)
        {
            throw new System.NotImplementedException();
        }

        Task ILocalStorageService.RemoveItemAsync(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}
