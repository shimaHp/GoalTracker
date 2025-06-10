using GoalTracker.Application.WorkItems.Dtos;
using GoalTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalTracker.Application.Goals.Dtos
{
    public class UpdateGoalWithWorkItemsDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = default!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public DateTime? TargetDate { get; set; }

        public GoalStatus Status { get; set; }

        public Priority Priority { get; set; }

        // Single collection with operation indicators
        public List<WorkItemUpdateDto>? WorkItems { get; set; }

        public byte[]? RowVersion { get; set; }
    }
}
