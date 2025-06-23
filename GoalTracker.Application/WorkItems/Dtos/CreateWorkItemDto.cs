using GoalTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalTracker.Application.WorkItems.Dtos
{
    public class CreateWorkItemDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = default!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public WorkItemStatus Status { get; set; } = WorkItemStatus.NotStarted;

        public Priority Priority { get; set; } = Priority.Medium;

        public string? AssigneeId { get; set; }
    }

}
