using System.ComponentModel.DataAnnotations;

namespace GoalTracker.UI.Blazor.Models.ViewModels
{
    public class CreateWorkItemViewModel
    {


        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public WorkItemStatus Status { get; set; } = WorkItemStatus.Todo;

        //public string? AssigneeId { get; set; }
   
    }
}