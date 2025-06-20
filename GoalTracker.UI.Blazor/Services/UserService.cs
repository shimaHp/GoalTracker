using AutoMapper;
using Blazored.LocalStorage;
using Blazored.Toast.Services;
using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Models.ViewModels.UsersViewModel;
using GoalTracker.UI.Blazor.Services.Base;
using System.Text.Json;

namespace GoalTracker.UI.Blazor.Services
{
    public class UserService : BaseHttpService, IUserService
    {
        private readonly IMapper _mapper;
        private readonly IToastService _toastService;

        public UserService(
            IMapper mapper,
            IToastService toastService,
            IClient client,
            Blazored.LocalStorage.ILocalStorageService localStorage)
            : base(client, localStorage)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _toastService = toastService;
        }

        public async Task<List<CollaboratorViewModel>> GetCollaboratorsAsync()
        {
            try
            {
                await AddBearerToken();

                var httpClient = _client.HttpClient;
                var response = await httpClient.GetAsync("api/identity/byrole/Collaborator");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var users = JsonSerializer.Deserialize<List<CollaboratorViewModel>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return users ?? new List<CollaboratorViewModel>();
                }

                _toastService.ShowWarning("Failed to load collaborators.");
                return new List<CollaboratorViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to fetch collaborators: {ex.Message}");
                _toastService.ShowError("Unexpected error occurred while fetching collaborators.");
                return new List<CollaboratorViewModel>();
            }
        }

        public async Task<List<CollaboratorViewModel>> GetUsersByRoleAsync(string role)
        {
            try
            {
                await AddBearerToken();

                var httpClient = _client.HttpClient;
                var response = await httpClient.GetAsync($"api/users/byrole/{role}");

                if (!response.IsSuccessStatusCode)
                {
                    _toastService.ShowWarning($"Failed to load users for role: {role}");
                    return new List<CollaboratorViewModel>();
                }

                var content = await response.Content.ReadAsStringAsync();
                var users = JsonSerializer.Deserialize<List<CollaboratorViewModel>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return users ?? new List<CollaboratorViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users for role '{role}': {ex.Message}");
                _toastService.ShowError($"Unexpected error occurred while loading users for role '{role}'.");
                return new List<CollaboratorViewModel>();
            }
        }
    }
}
