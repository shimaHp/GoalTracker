namespace GoalTracker.UI.Blazor.Models.ViewModels
{
    public class GoalsVM
    {

        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? TargetDate { get; set; }
       // public GoalStatus Status { get; set; }
        //public Priority Priority { get; set; }
        // Collection of WorkItems related to this Goal
        public ICollection<WorkItemVM> WorkItems { get; set; } = new List<WorkItemVM>();
        public string UserId { get; set; } = default!;
       // public User User { get; set; } = default!;
    }
}
