using GoalTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalTracker.Application.WorkItems.Dtos
{
    public class UpdateWorkItemDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = default!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public WorkItemStatus Status { get; set; }



        public string? AssigneeId { get; set; }

        // Optional: Version for optimistic concurrency control
        public byte[]? RowVersion { get; set; }
    }
}
