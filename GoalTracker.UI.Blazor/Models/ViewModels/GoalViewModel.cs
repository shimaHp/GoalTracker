using GoalTracker.UI.Blazor.Models.Enums;
using GoalTracker.UI.Blazor.Models.ViewModels.WorkItems;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoalTracker.UI.Blazor.Models.ViewModels
{
    public class GoalViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; } = string.Empty;
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string? Description { get; set; }

        // Change these to DateTimeOffset
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? TargetDate { get; set; }

        public GoalStatus Status { get; set; }
        public Priority Priority { get; set; }
        public ICollection<WorkItemViewModel> WorkItems { get; set; } = new List<WorkItemViewModel>();
        public string UserId { get; set; } = string.Empty;
    }}
