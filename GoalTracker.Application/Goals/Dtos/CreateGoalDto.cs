using GoalTracker.Application.WorkItems.Dtos;
using GoalTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalTracker.Application.Goals.Dtos
{
    public class CreateGoalDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? TargetDate { get; set; }
        public GoalStatus Status { get; set; }
        public Priority Priority { get; set; }
        // Collection of WorkItems related to this Goal

        public List<CreateWorkItemDto> WorkItems { get; set; } = [];
    }
}
