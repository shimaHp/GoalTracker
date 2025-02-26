using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Services.Base;

namespace GoalTracker.UI.Blazor.Services
{
    public class WorkItemService : BaseHttpService, IWorkItemService
    {
        public WorkItemService(IClient client) : base(client)
        {
        }
    }
}
