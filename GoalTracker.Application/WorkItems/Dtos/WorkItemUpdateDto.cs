using GoalTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalTracker.Application.WorkItems.Dtos
{
    public class WorkItemUpdateDto
    {
        public int Id { get; set; } // 0 for new items

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = default!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public WorkItemStatus Status { get; set; }

        public Priority Priority { get; set; }

        public int? AssigneeId { get; set; }

        // Operation flag
        public WorkItemOperation Operation { get; set; } = WorkItemOperation.Update;

        public byte[]? RowVersion { get; set; }
    }
    // Enum for work item operations
    public enum WorkItemOperation
    {
        Create,
        Update,
        Delete
    }
}
