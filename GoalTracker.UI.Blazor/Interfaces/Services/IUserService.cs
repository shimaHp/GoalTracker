using GoalTracker.UI.Blazor.Models.ViewModels.UsersViewModel;

namespace GoalTracker.UI.Blazor.Interfaces.Services
{
    public interface IUserService
    {
        Task<List<CollaboratorViewModel>> GetUsersByRoleAsync(string role);
        Task<List<CollaboratorViewModel>> GetCollaboratorsAsync();
    }
}
