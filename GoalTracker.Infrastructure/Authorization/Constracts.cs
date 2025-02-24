
namespace GoalTracker.Infrastructure.Authorization;

public static class PolicyNames
{
 
    public const string AtLeast18 = "AtLeast18";
    public const string CreatedAtLeast2Goals = "CreatedAtLeast";

}
public static class AppClaimTypes
{
  
    public const string DateOfBirth = "DateOfBirth";
}


  

    // Goal and WorkItem specific policies
    public static class ResourcePolicies
{
    public static class Goal
    {
        // Create a new goal
        public const string Create = "Goal.Create";

        // Read any goal
        public const string Read = "Goal.Read";

        // Update own goals
        public const string Update = "Goal.Update";

        // Delete own goals
        public const string Delete = "Goal.Delete";

        // Admin-level goal management
        public const string Manage = "Goal.Manage";
    }

    public static class WorkItem
    {
        // Create a work item
        public const string Create = "WorkItem.Create";

        // Read work items
        public const string Read = "WorkItem.Read";

        // Update own work items
        public const string Update = "WorkItem.Update";

        // Delete own work items
        public const string Delete = "WorkItem.Delete";

        // Admin-level work item management
        public const string Manage = "WorkItem.Manage";
    }
}


