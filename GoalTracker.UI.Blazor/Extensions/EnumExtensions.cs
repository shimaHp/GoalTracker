namespace GoalTracker.UI.Blazor.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this GoalStatus status)
        {
            return status switch
            {
                GoalStatus.NotStarted => "Not Started",
                GoalStatus.InProgress => "In Progress",
                GoalStatus.Completed => "Completed",
                GoalStatus.Cancelled => "Cancelled",
                _ => status.ToString(),
            };
        }

        public static string GetCssClass(this GoalStatus status)
        {
            return status switch
            {
                GoalStatus.NotStarted => "status-not-started",
                GoalStatus.InProgress => "status-in-progress",
                GoalStatus.Completed => "status-completed",
                GoalStatus.Cancelled => "status-abandoned",
                _ => string.Empty,
            };
        }

        // Similar methods for Priority
        public static string GetDisplayName(this Priority priority)
        {
            return priority.ToString();
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
        public static string GetDisplayName(this WorkItemStatus status)
        {
            return status switch
            {
                WorkItemStatus.Todo => "To Do",
                WorkItemStatus.InProgress => "In Progress",
                WorkItemStatus.Blocked => "Blocked",
                WorkItemStatus.Completed => "Completed",
                WorkItemStatus.Cancelled => "Cancelled",
                _ => status.ToString(),
            };
        }

        public static string GetCssClass(this WorkItemStatus status)
        {
            return status switch
            {
                WorkItemStatus.Todo => "status-todo",
                WorkItemStatus.InProgress => "status-in-progress",
                WorkItemStatus.Blocked => "status-blocked",
                WorkItemStatus.Completed => "status-completed",
                WorkItemStatus.Cancelled => "status-cancelled",
                _ => string.Empty,
            };
        }
    }
}
