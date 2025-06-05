using Microsoft.JSInterop;
using System.Text.Json;
using System.Threading.Tasks;

namespace GoalTracker.UI.Blazor.Interfaces.Services
{
    public interface ILocalStorageService
    {
        Task<T> GetItemAsync<T>(string key);
        Task SetItemAsync<T>(string key, T value);
        Task RemoveItemAsync(string key);
    }

    
}