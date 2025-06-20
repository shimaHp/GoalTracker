using GoalTracker.UI.Blazor.Models.Enums;
using ServiceBase = GoalTracker.UI.Blazor.Services.Base;

namespace GoalTracker.UI.Blazor.Extensions
{
    public static class EnumExtensions
    {
        #region Display Names
        public static string GetDisplayName(this GoalStatus status)
        {
            return status switch
            {
                GoalStatus.NotStarted => "Not Started",
                GoalStatus.InProgress => "In Progress",
                GoalStatus.Completed => "Completed",
                GoalStatus.OnHold => "On Hold",
                // Remove Cancelled if not in domain, or add it to domain
                _ => status.ToString(),
            };
        }

        public static string GetDisplayName(this Priority priority)
        {
            return priority.ToString();
        }

        public static string GetDisplayName(this WorkItemStatus status)
        {
            return status switch
            {
                WorkItemStatus.NotStarted => "To Do",
                WorkItemStatus.InProgress => "In Progress",
                WorkItemStatus.Blocked => "Blocked",
                WorkItemStatus.Completed => "Completed",
                WorkItemStatus.Cancelled => "Cancelled",
                _ => status.ToString(),
            };
        }
        #endregion

        #region CSS Classes
        public static string GetCssClass(this GoalStatus status)
        {
            return status switch
            {
                GoalStatus.NotStarted => "status-not-started",
                GoalStatus.InProgress => "status-in-progress",
                GoalStatus.Completed => "status-completed",
                GoalStatus.OnHold => "status-on-hold",
                // Remove Cancelled if not in domain
                _ => string.Empty,
            };
        }

        public static string GetCssClass(this Priority priority)
        {
            return priority switch
            {
                Priority.Low => "priority-low",
                Priority.Medium => "priority-medium",
                Priority.High => "priority-high",
                Priority.Critical => "priority-critical",
                _ => string.Empty,
            };
        }

        public static string GetCssClass(this WorkItemStatus status)
        {
            return status switch
            {
                WorkItemStatus.NotStarted => "status-todo",
                WorkItemStatus.InProgress => "status-in-progress",
                WorkItemStatus.Blocked => "status-blocked",
                WorkItemStatus.Completed => "status-completed",
                WorkItemStatus.Cancelled => "status-cancelled",
                _ => string.Empty,
            };
        }
        #endregion

     

    }
}